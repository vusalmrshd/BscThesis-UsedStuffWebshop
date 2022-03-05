using System;
using System.Threading.Tasks;
using Core.Interface;
using Microsoft.Extensions.Configuration;
using Models.Database.Context;
using Models.Request.Order;
using Models.Response.CoreResponse;
using Models.Utils;
using Stripe;

namespace Core.Services
{
    public class StripeService : IStripeService
    {
        private ICoreResponseModel _response;
        private IConfiguration _config;
        private DatabaseContext _context;
        private ICommonService _common;
        private IEmailService _emailService;
        private IProcedures _sps;



        public StripeService(ICoreResponseModel _response, IConfiguration _config, DatabaseContext _context, ICommonService _common, IProcedures _sps, IEmailService _emailService)
        {
            this._response = _response;
            this._config = _config;
            this._context = _context;
            this._common = _common;
            this._emailService = _emailService;
            this._sps = _sps;
            StripeConfiguration.ApiKey = _config["Stripe:secretKey"];


        }

        public async Task<CoreResponseModel> createCharge(PlaceOrderRequest request,decimal bill,string customerName,string sellerId)
        {
            try
            {
                var token = await createToken(request);
                if (token.success)
                {
                    int amount = Convert.ToInt32(bill * 100);
                    var options = new ChargeCreateOptions
                    {
                        Amount = amount,
                        Currency = "usd",
                        Source = ( (Token)token.data).Id,
                        Description = $"{customerName} has purchased the product from seller having sellerId {sellerId}",
                    };
                    var service = new ChargeService();
                    await service.CreateAsync(options);


                    return _response.getSuccessResponse(_MESSAGES.success, null);
                }
                else
                    return token;
                
            }
            catch (Exception e)
            {
                return _response.getFailResponse(e.Message, null);
            }
        }


        public async Task<CoreResponseModel> createToken(PlaceOrderRequest card)
        {
            try
            {
                var options = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = card.cardNumber,
                        ExpMonth = card.expMonth,
                        ExpYear = card.expYear,
                        Cvc = card.cvc+"",
                    },
                };
                var service = new TokenService();
                var tokenResponse = await service.CreateAsync(options).ConfigureAwait(false);
              
                return _response.getSuccessResponse(_MESSAGES.success, tokenResponse);
            }
            catch (Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }
    }
}
