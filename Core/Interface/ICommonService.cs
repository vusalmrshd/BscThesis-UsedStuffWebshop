using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Models.Response.CoreResponse;

namespace Core.Interface
{
    public interface ICommonService
    {
        string passwordHasher(string password);
        Task<CoreResponseModel> checkEmailExists(string email);
        Task<CoreResponseModel> checkUserNameExists(string userName);
        Task<CoreResponseModel> saveImageToServer(IFormFile file);
        Task<Stream> downloadFileFromServer(string path, string fileName);
        bool deleteFileIfExists(string path, bool isImage, bool isPpt);
    }
}
