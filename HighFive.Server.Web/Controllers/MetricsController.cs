using HighFive.Server.Services.Models;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HighFive.Server.Services.Utils;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HighFive.Server.Web.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class MetricsController : Controller
    {
        private readonly IHighFiveRepository _repository;
        private readonly ILogger<MetricsController> _logger;

        public MetricsController(IHighFiveRepository repository, ILogger<MetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET api/metrics/Ariel Partners
        [HttpGet("{orgName}")]
        public IActionResult GetMetricsByOrganization(string orgName)
        {
            var org = _repository.GetOrganizationByName(orgName);
            if (org == null) return NotFound(new { Message = $"Organization {orgName} not found" });
            var viewModel = new MetricsViewModel();
            try
            {
                viewModel.Week = _repository.GetMetrics(orgName, 7).ToList();
                viewModel.Month = _repository.GetMetrics(orgName, 30).ToList();
                viewModel.Year = _repository.GetMetrics(orgName, 365).ToList();
                viewModel.ToDate = _repository.GetMetrics(orgName, (DateTime.Now - new DateTime(DateTime.Now.Year, 1, 1)).Days).ToList();
                return Ok(viewModel);
            }
            catch (HighFiveException ex)
            {
                _logger.LogError($"Failed to get metrics for {orgName}: {ex}");
            }
            return BadRequest(new { Message = $"Failed to get metrics for Organization {orgName}" });
        }
    }
}
