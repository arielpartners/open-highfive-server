using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HighFive.Server.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RecognitionController : Controller
    {
        private readonly IHighFiveRepository _repository;
        private readonly ILogger<RecognitionController> _logger;

        #region Constructor

        public RecognitionController(IHighFiveRepository repository, ILogger<RecognitionController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #endregion

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var recognitions = _repository.GetAllRecognitions().ToList();
                if (recognitions.Count > 0) return Ok(Mapper.Map<List<RecognitionViewModel>>(recognitions));
                return NoContent();
            }
            catch (DbException ex)
            {
                _logger.LogError("Failed to get recognitions: {0}", ex);
            }
            return BadRequest(new { Message = "Failed to get recognitions" });
        }
    }
}
