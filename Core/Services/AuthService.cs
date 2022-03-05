using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Database.Context;
using Models.Database.Entity;
using Models.Database.Sps;
using Models.Request.Auth;
using Models.Response.Auth;
using Models.Response.CoreResponse;
using Models.Utils;

namespace Core.Services
{
    public class AuthService : IAuthService
    {
        private ICoreResponseModel _response;
        private IConfiguration _config;
        private DatabaseContext _context;
        private ICommonService _common;
        private IEmailService _emailService;
        private IProcedures _sps;


        public AuthService(ICoreResponseModel _response,IConfiguration _config,DatabaseContext _context,ICommonService _common,IProcedures _sps,IEmailService _emailService)
        {
            this._response = _response;
            this._config = _config;
            this._context = _context;
            this._common = _common;
            this._emailService = _emailService;
            this._sps = _sps;
        }


        public async Task<CoreResponseModel> register(Register register)
        {
            try
            {
                if (register.status == "1")
                    register.isSeller = true;
                else
                    register.isSeller = false;
                var checkMailResult = await _common.checkEmailExists(register.email);
                if(!checkMailResult.success) return checkMailResult;

                var checkUsernameResult = await _common.checkUserNameExists(register.userName);
                if (!checkUsernameResult.success) return checkUsernameResult;

                string imageLink = "";
                if(register.image != null)
                {
                    var imageResult = await _common.saveImageToServer(register.image).ConfigureAwait(false);
                    if (!imageResult.success) return imageResult;
                    imageLink = $"{imageResult.data}";
                }

                var userToCreate = new User()
                {
                    email = register.email,
                    userName = register.userName,
                    name = register.name,
                    isSeller=register.isSeller,
                    profileImage = imageLink,
                    password = _common.passwordHasher(register.password),
                    createdAt = DateTimeOffset.UtcNow,
                    updatedAt = DateTimeOffset.UtcNow,
                    isActive = true
                };

                userToCreate.createdBy = userToCreate.updatedBy = userToCreate.userId;
                await _context.users.AddAsync(userToCreate).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                var permissions = new  List<Permission>();
                if (!register.isSeller)
                {
                    await _context.userRoles.AddAsync(new UserRole()
                    {
                        roleId = Convert.ToInt64(_SYSCONSTS.roleBuyer[0]),
                        userId = userToCreate.userId
                    }).ConfigureAwait(false);
                    foreach (var perm in _SYSCONSTS.buyerPermissions)
                    {
                        await _context.userPermissions.AddAsync(new UserPermission()
                        {
                            permissionId = Convert.ToInt64(perm),
                            userId = userToCreate.userId
                        }).ConfigureAwait(false);
                    }
                }
                else
                {
                    await _context.userRoles.AddAsync(new UserRole()
                    {
                        roleId = Convert.ToInt64(_SYSCONSTS.roleSeller[0]),
                        userId = userToCreate.userId
                    }).ConfigureAwait(false);
                    foreach (var perm in _SYSCONSTS.sellerPermissions)
                    {
                        await _context.userPermissions.AddAsync(new UserPermission()
                        {
                            permissionId = Convert.ToInt64(perm),
                            userId = userToCreate.userId
                        }).ConfigureAwait(false);
                    }
                }

                userToCreate.createdBy = userToCreate.updatedBy = userToCreate.userId;
                await _context.SaveChangesAsync().ConfigureAwait(false);

                var user = await _context.users.FirstOrDefaultAsync(x => x.email == register.email).ConfigureAwait(false);
                if (user == null) return _response.getFailResponse(_MESSAGES.noRecordExist, null);
                string token = generateJWT(user.userId, user.userName, !user.isSeller ? _SYSCONSTS.roleBuyer[0] : _SYSCONSTS.roleSeller[0]);
                bool result = await _emailService.sendEmailForActivateAccount(register.email, token,register.link, user.name);
                if (!result) return _response.getFailResponse(_MESSAGES.errorInSendingEmail, null);

                await _context.userForgetPasswords.AddAsync(new UserForgetPassword()
                {
                    //id = Guid.NewGuid(),
                    email = register.email,
                    code = token
                }).ConfigureAwait(false);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return _response.getSuccessResponse(_MESSAGES.success, null);

            }
            catch(Exception e)
            {
                return _response.getFailResponse(e.Message, null);
            }
        }

        public async Task<CoreResponseModel> activateAccount(string email)
        {
            try
            {
                var user = await _context.users.FirstOrDefaultAsync(x => x.email == email).ConfigureAwait(false);
                if(user != null)
                {
                    user.isActive = true;
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    return _response.getSuccessResponse("Account Activated", null);
                }
                return _response.getFailResponse(_MESSAGES.noRecordExist, null);
            }
            catch(Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }

        public async Task<CoreResponseModel> login(Login login)
        {
            try
            {
                var user = await _sps.spLoginUser(login).ConfigureAwait(false);
                if (user == null) return _response.getFailResponse(_MESSAGES.incorrectCredentials, null);

                var userPermissions = await _sps.spGetUserPermissions(user.userId).ConfigureAwait(false);
                return createLoginResponse(user, userPermissions);
                
            }
            catch(Exception e)
            {
                return _response.getFailResponse(e.Message, null);
            }
        }

        public async Task<CoreResponseModel> updateUser(Register register)
        {
            try
            {
                var user = await _context.users.FirstOrDefaultAsync(x => x.userId == register.userId).ConfigureAwait(false);
                if (user == null) return _response.getFailResponse(_MESSAGES.noRecordExist, null);

                if (register.userName != null)
                {
                    var userNameExistsResult = await _common.checkUserNameExists(register.userName).ConfigureAwait(false);
                    if (!userNameExistsResult.success && user.userName != register.userName) return userNameExistsResult;
                }

                if (register.image != null)
                {
                    _common.deleteFileIfExists(user.profileImage, true, false);
                    var imageResult = await _common.saveImageToServer(register.image).ConfigureAwait(false);
                    if (!imageResult.success) return imageResult;
                    user.profileImage = imageResult.data + "";
                }
                user.updatedAt = DateTimeOffset.UtcNow;
                user.updatedBy = register.createdBy;
                user.name = register.name == null ? user.name : register.name;
                user.userName = register.userName == null ? user.userName : register.userName;
                user.password = register.password == null ? user.password : _common.passwordHasher(register.password);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.success, user);
            }
            catch (Exception e)
            {
                return _response.getFailResponse(e.Message, null);
            }
        }

        public async Task<CoreResponseModel> forgetPassword(ForgetPassowrd forget)
        {
            try
            {
                var user = await _context.users.FirstOrDefaultAsync(x => x.email == forget.email && x.isActive == true).ConfigureAwait(false);
                if (user == null) return _response.getFailResponse(_MESSAGES.noRecordExist, null);
                string token = generateJWT(user.userId, user.userName,!user.isSeller? _SYSCONSTS.roleBuyer[0] : _SYSCONSTS.roleSeller[0]);
                bool result = await _emailService.sendEmailForUpdatePassword(forget.email, token,forget.link,user.name);
                if (!result) return _response.getFailResponse(_MESSAGES.errorInSendingEmail, null);

                await _context.userForgetPasswords.AddAsync(new UserForgetPassword()
                {
                    //id = Guid.NewGuid(),
                    email = forget.email,
                    code = token
                }).ConfigureAwait(false);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return _response.getSuccessResponse(_MESSAGES.emailSent, null);
            }
            catch (Exception e)
            {
                return _response.getFailResponse(_MESSAGES.noRecordExist, null);
            }
        }



        public async Task<CoreResponseModel> validatePasswordLink(ForgetPassowrd update)
        {
            try
            {
                var userCodeInDb = await _context.userForgetPasswords.FirstOrDefaultAsync(x => x.code == update.token).ConfigureAwait(false);
                if (userCodeInDb == null) return _response.getFailResponse(_MESSAGES.noRecordExist, null);
                if (userCodeInDb.code != update.token) return _response.getFailResponse(_MESSAGES.incorrectCode, null);

                return _response.getSuccessResponse(_MESSAGES.passwordUpdated, null);
            }
            catch (Exception e)
            {
                return _response.getFailResponse(_MESSAGES.passwordUpdatefailed, null);
            }
        }

        public async Task<CoreResponseModel> updatePassword(UpdatePassword update)
        {
            try
            {
                var userCodeInDb = await _context.userForgetPasswords.FirstOrDefaultAsync(x => x.code == update.code).ConfigureAwait(false);
                if (userCodeInDb == null) return _response.getFailResponse(_MESSAGES.noRecordExist, null);
                
                var user = await _context.users.FirstOrDefaultAsync(x => x.email == userCodeInDb.email && x.isActive == true).ConfigureAwait(false);
                if (user == null) return _response.getFailResponse(_MESSAGES.noRecordExist, null);

                user.password = _common.passwordHasher(update.password);

                _context.userForgetPasswords.Remove(userCodeInDb);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.passwordUpdated, null);
            }
            catch(Exception e)
            {
                return _response.getFailResponse(_MESSAGES.passwordUpdatefailed, null);
            }
        }

        private CoreResponseModel createLoginResponse(SpLoginResponse user,List<SpPermissionResponse> permissions)
        {
            try
            {   
                var loginResponse = new LoginResponseModel()
                {
                    userId = user.userId,
                    name = user.name,
                    userName = user.userName,
                    isSeller = user.isSeller,
                    role = user.roleName,
                    email=user.email,
                    createdAt = DateTime.UtcNow,
                    token = generateJWT(user.userId, user.userName,user.roleName),
                    permissions = permissions
                };

                return _response.getSuccessResponse(_MESSAGES.success, loginResponse);

            }
            catch (Exception e)
            {
                return _response.getFailResponse(e.Message, null);
            }
        }

        private string generateJWT(long userId,string userName,string role)
        {
            try
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid,$"{userId}"),
                    new Claim(ClaimTypes.Role,role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:secret"]));

                var token = new JwtSecurityToken(
                    issuer: _config["jwt:validIssuer"],
                    audience: _config["jwt:validAudience"],
                    expires: DateTime.Now.AddDays(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    ); 

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        
    }
}
