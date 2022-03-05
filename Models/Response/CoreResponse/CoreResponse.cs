using System;
using Models.Utils;

namespace Models.Response.CoreResponse
{
    public interface ICoreResponseModel
    {
        CoreResponseModel getSuccessResponse(string message, object data);
        CoreResponseModel getFailResponse(string message, object data);
        CoreResponseModel getUnAuthorizedResponse();
        CoreResponseModel getNotAllowedResponse();
    }
    public class CoreResponseModel : ICoreResponseModel
    {
        public string message { get; set; }
        public int status { get; set; }
        public bool success { get; set; }
        public object data { get; set; }

        public CoreResponseModel getSuccessResponse(string message, object data)
        {
            this.message = message;
            this.data = data;
            status = 200;
            success = true;
            return this;
        }

        public CoreResponseModel getFailResponse(string message, object data)
        {
            this.message = message;
            this.data = data;
            status = 400;
            success = false;
            return this;
        }

        public CoreResponseModel getUnAuthorizedResponse()
        {
            message = _MESSAGES.unAuthorize;
            data = null;
            status = 401;
            success = false;
            return this;
        }

        public CoreResponseModel getNotAllowedResponse()
        {
            message = _MESSAGES.unAvailable;
            data = null;
            status = 451;
            success = false;
            return this;
        }

    }

}
