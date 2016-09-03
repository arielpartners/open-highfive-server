using System.Data.Common;
using System.Threading.Tasks;
using HighFive.Server.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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


        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> Ping()
        {
            try
            {
                await _repository.IsConnected();
                return Ok("Health Check - App Service and backend - Good");
            }
            catch (DbException ex)
            {
                _logger.LogError("Failed to Connect to backend: {0}", ex);
            }
            return BadRequest(new { Message = "Failed Connecting to backend from API" });
        }
    }
}
