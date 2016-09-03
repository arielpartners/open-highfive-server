using System.Data.Common;
using System.Threading.Tasks;
using HighFive.Server.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;

namespace HighFive.Server.Web.Controllers
{
    [Route("api/[controller]")]
    public class HealthCheckController : Controller
    {
        private readonly IHighFiveRepository _repository;
        private readonly ILogger<HealthCheckController> _logger;
        private readonly IConfigurationRoot _config;

        #region Constructor

        public HealthCheckController(IConfigurationRoot config, IHighFiveRepository repository, ILogger<HealthCheckController> logger)
        {
            _repository = repository;
            _logger = logger;
            _config = config;
        }

        #endregion

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Ping()
        {
            try
            {
                await _repository.IsConnected();
                var version = _config["version"];
                var healthcheck = new
                {
                    Healthy = true,
                    Message = "Health Check - App Service and backend - Good",
                    Version = version,
                    Datestamp = FileLastWriteTime
                };
                return Ok(healthcheck);
            }
            catch (DbException ex)
            {
                _logger.LogError("Failed to Connect to backend: {0}", ex);
            }
            return BadRequest(new { Message = "Failed Connecting to backend from API" });
        }

        private static DateTime FileLastWriteTime
        {
            get
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var fileInfo = new System.IO.FileInfo(assembly.Location);
                return fileInfo.LastWriteTime.ToUniversalTime();
            }
        }
    }
}
