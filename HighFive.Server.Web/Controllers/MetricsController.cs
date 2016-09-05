#region references

using AutoMapper;
using System.Threading.Tasks;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using HighFive.Server.Services.Utils;
using HighFive.Server.Web.Utils;
using System;

#endregion

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HighFive.Server.Web.Controllers
{
    [Route("api/[controller]")]
    public class MetricsController : Controller
    {
        private readonly IHighFiveRepository _repository;
        private readonly ILogger<MetricsController> _logger;
        private readonly IWrapSignInManager<HighFiveUser> _signInManager;

        public MetricsController(IHighFiveRepository repository, ILogger<MetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET api/metrics/arielpartners
        [HttpGet("{webpath}")]
        public IActionResult GetMetricsByOrganization(string orgName)
        {
            var org = _repository.GetOrganizationByName(orgName);
            if (org == null)
            {
                return NotFound(new { Message = $"Organization {orgName} not found" });
            }

            MetricsViewModel viewModel = new MetricsViewModel();
            try
            {
                var weekMetrics = _repository.GetMetrics(orgName, 7);
                if (weekMetrics.Count > 0)
                    viewModel.Week = weekMetrics;
                var MonthMetrics = _repository.GetMetrics(orgName, 30 );
                if (MonthMetrics.Count > 0)
                    viewModel.Month = MonthMetrics;
                var yearMetrics = _repository.GetMetrics(orgName, 365);
                if (yearMetrics.Count > 0)
                    viewModel.Year = yearMetrics;

                int thisYear = DateTime.Now.Year;
                DateTime thisJanFirst = new DateTime(thisYear, 1, 1);
                int daysToJanFirst = (DateTime.Now - thisJanFirst).Days;

                var toDateMetrics = _repository.GetMetrics(orgName, daysToJanFirst);
                if (toDateMetrics.Count > 0)
                    viewModel.ToDate = toDateMetrics;
                return Ok(viewModel);
                
            }
            catch (HighFiveException ex)
            {
                _logger.LogError($"Failed to get metrics for {orgName} : {ex}");
            }
            return BadRequest(new { Message = "Failed to get metrics" });
        }


    } 
}
