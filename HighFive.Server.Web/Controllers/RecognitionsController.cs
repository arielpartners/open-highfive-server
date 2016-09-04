using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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
    public class RecognitionsController : Controller
    {
        private readonly IHighFiveRepository _repository;
        private readonly ILogger<RecognitionsController> _logger;

        #region Constructor

        public RecognitionsController(IHighFiveRepository repository, ILogger<RecognitionsController> logger)
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
