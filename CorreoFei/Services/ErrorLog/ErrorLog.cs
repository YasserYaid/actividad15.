using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace CorreoFei.Services.ErrorLog
{
    public class ErrorLog : IErrorLog
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httocontextAccessor;

        public ErrorLog(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httocontextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httocontextAccessor = httocontextAccessor;
        }

        [HttpPost]
        public async Task ErrorLogAsync(string Mensaje)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                string path = "";
                path = Path.Combine(webRootPath, "log");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //Retrieve server/local IP address
                var feature = _httocontextAccessor.HttpContext.Features.Get<IHttpConnectionFeature>();
                string LocalIPAddr = feature?.LocalIpAddress?.ToString();
                //Write the especified text asynchronously to a new file named "WriteTextAsync.txt"
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "log.txt"), true))
                {
                    await outputFile.WriteLineAsync(Mensaje + " - " +
                        _httocontextAccessor.HttpContext.User.Identity.Name +
                        " - " + LocalIPAddr + " - " + DateTime.Now.ToString());
                }
            }
            catch (Exception e)
            {
                //No hace nada
            }
        }
    }
}
