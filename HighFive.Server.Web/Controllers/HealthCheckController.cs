using System.Data.Common;
using System.Threading.Tasks;
using HighFive.Server.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace HighFive.Server.Web.Controllers
{
    [Route("api/[controller]")]
    public class HealthCheckController : Controller
    {
        private readonly IHighFiveRepository _repository;
        private readonly ILogger<HealthCheckController> _logger;

        #region Constructor

        public HealthCheckController(IHighFiveRepository repository, ILogger<HealthCheckController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #endregion

        //
        // "healthcheck": {
        //  "healthy": true,
        //  "message": "Health Check - App Service and backend - Good",
        //  "version": "1.2",
        //  "datestamp": "2016-09-03T14:23:00.000Z"
        //},
        //
        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> Ping()
        {
            try
            {
                await _repository.IsConnected();
                var healthcheck = new
                {
                    Healthy = true,
                    LoggerMessage = "Health Check - App Service and backend - Good",
                    Version = "1.2",
                    Datestamp = DateTime.UtcNow

                };
                return Ok(healthcheck);
            }
            catch (DbException ex)
            {
                _logger.LogError("Failed to Connect to backend: {0}", ex);
            }
            return BadRequest(new { Message = "Failed Connecting to backend from API" });
        }

        private string AppReleaseDate
        {
            get
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var fileInfo = new System.IO.FileInfo(assembly.Location);
                return fileInfo.LastWriteTime.ToString("MM/dd/yyyy hh:mm:ss tttt");
            }
        }
    }
}
