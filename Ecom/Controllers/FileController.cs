using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Ecom.Controllers.APi
{
    [Route("api/[controller]")]
    public class FileController : BaseController
    {

        [HttpGet("download")]
        public async Task<IActionResult> getFileFromServer(string path, string fileName)
        {
            var memory = await _CommonService.downloadFileFromServer(path, fileName);
            if (memory == null) return NotFound("No File Exists");
            return File(memory, "image/png");
        }
    }
}
