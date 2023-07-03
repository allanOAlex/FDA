using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Sinks.File;
using TB.Application.Abstractions.Interfaces;
using TB.Mvc.Extensions;
using TB.Mvc.Middleware;
using TB.Shared.Exceptions.Global;
using TB.Shared.Requests.Logging;
using TB.Shared.Responses.Logging;

namespace TB.Mvc.Controllers
{
    public class LogController : Controller
    {
        private IConfiguration config;
        private readonly IServiceManager serviceManager;
        private readonly string FolderPath;
        private readonly string Format;
        private string[] Files;
        private readonly string LogFile;



        public LogController(IConfiguration Configuration, IServiceManager ServiceManager)
        {
            config = Configuration;
            serviceManager = ServiceManager;
            FolderPath = config["Logging:Serilog:FolderPath"]!;
            Format = config["Logging:Serilog:Format"]!;
            LogFile = config["Logging:Serilog:LogFile"]!;
        }
        public IActionResult Logs()
        {
            return View();
        }

        public async Task<ActionResult<List<QueryLogFileResponse>>> QueryFromFile(QueryLogFileRequest queryLogFileRequest)
        {
            ViewBag.Caption = "Logs";

            try
            {
                Files = Directory.GetFiles(FolderPath, "*.*").Where(file => ClientExtensions.IsFileFormatMatch(file, Format)).ToArray();
                if (Files.Length > 0)
                {
                    string newestFile = Files.OrderByDescending(file => System.IO.File.GetLastWriteTime(file)).FirstOrDefault()!;

                    if (newestFile != null)
                    {
                        if (new FileInfo(newestFile).Length > 0)
                        {
                            queryLogFileRequest.LogFile = newestFile;
                            var response = await serviceManager.LoggingService.QueryFromFile(queryLogFileRequest);
                            if (response.Count() <= 0)
                            {
                                return Json(response);
                            }

                            return View(response);
                        }
                        else
                        {
                            throw new EmptyFileException();
                        }
                    }
                    else { throw new FileDoesNotExistException(); }
                }
                else
                {
                    throw new FileNotFoundException();
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            

            
        }

        public async Task<ActionResult<List<QueryLogFileResponse>>> QueryLogFile(QueryLogFileRequest queryLogFileRequest)
        {
            queryLogFileRequest.LogFile = LogFile;
            var response = await serviceManager.LoggingService.QueryLogFile(queryLogFileRequest);
            if (response.Count() !> 0)
            {
                return Json(response);
            }

            return Json(response);
        }



    }
}
