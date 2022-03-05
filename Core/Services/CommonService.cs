using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Database.Context;
using Models.Response.CoreResponse;
using Models.Utils;

namespace Core.Services
{
    public class CommonService : ICommonService
    {
        private ICoreResponseModel _response;
        private IConfiguration _config;
        private DatabaseContext _context;

        public CommonService(ICoreResponseModel _response, DatabaseContext _context, IConfiguration _config)
        {
            this._response = _response;
            this._context = _context;
            this._config = _config;
        }

        public string passwordHasher(string password)
        {
            try
            {
                string hashedPassword = "";
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                }

                return hashedPassword;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<CoreResponseModel> checkEmailExists(string email)
        {
            try
            {
                var user = await _context.users.FirstOrDefaultAsync(x => x.email == email).ConfigureAwait(false);

                if (user != null) return _response.getFailResponse(_MESSAGES.emailExists, null);

                //user = await _context.users.FirstOrDefaultAsync(x => x.userName == userName).ConfigureAwait(false);

                return user == null ? _response.getSuccessResponse(_MESSAGES.success, null) : _response.getFailResponse(_MESSAGES.userNameExists, null);

            }
            catch (Exception e)
            {
                return _response.getFailResponse(e.Message, null);
            }
        }

        public async Task<CoreResponseModel> checkUserNameExists(string userName)
        {
            try
            {

                var user = await _context.users.FirstOrDefaultAsync(x => x.userName == userName).ConfigureAwait(false);

                return user == null ? _response.getSuccessResponse(_MESSAGES.success, null) : _response.getFailResponse(_MESSAGES.userNameExists, null);

            }
            catch (Exception e)
            {
                return _response.getFailResponse(e.Message, null);
            }
        }

        public bool deleteFileIfExists(string path,bool isImage,bool isPpt)
        {
            try
            {
                
                var rootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
                string fileName = path.Split(new string[] { "fileName=" }, StringSplitOptions.None)[1];
                if (isPpt)
                {
                    var filepath = Path.Combine(rootPath, "Resources", "file", fileName);
                    if (File.Exists(filepath))
                        File.Delete(filepath);
                }
                if (isImage)
                {
                    
                    var filepath = Path.Combine(rootPath, "Resources", "image", fileName);
                    if (File.Exists(filepath))
                        File.Delete(filepath);
                }
                else
                {
                    var filepath = Path.Combine(rootPath, "Resources", "video", fileName);
                    if (File.Exists(filepath))
                        File.Delete(filepath);
                }
                return true;
                
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<Stream> downloadFileFromServer(string path, string fileName)
        {
            try
            {
                var rootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
                var filepath = Path.Combine("wwwroot", "Resources", path, fileName);
                Stream memory = new MemoryStream();
                using (var stream = new FileStream(filepath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return memory;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CoreResponseModel> saveImageToServer(IFormFile file)
        {
            try
            {
                var extension = Path.GetExtension(file.FileName);
                var enviroment = Environment.CurrentDirectory;
                string projectDirectory = Directory.GetParent(enviroment).Parent.FullName;
                string folderName = "";
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    folderName = Path.Combine("wwwroot", "Resources", "image");
                else
                    return _response.getFailResponse("Allowed extension is .jpeg .jpg .png", null);

                var fileName = Guid.NewGuid().ToString() + extension;
                var fullPath = Path.Combine(folderName, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return _response.getSuccessResponse("Uploaded", _config["imageUploadPath"] + fileName);
            }
            catch (Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }


    }
}
