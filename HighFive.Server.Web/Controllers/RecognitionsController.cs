using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HighFive.Server.Services.Models;
using HighFive.Server.Services.Utils;
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

        [HttpGet("{id}")]
        public IActionResult GetRecognitionById(int id)
        {
            try
            {
                var recognition = _repository.GetRecognitionById(id);
                if (recognition!=null) return Ok(Mapper.Map<RecognitionViewModel>(recognition));
                return NoContent();
            }
            catch (DbException ex)
            {
                _logger.LogError("Failed to get recognition: {0}", ex);
            }
            return BadRequest(new { Message = $"Failed to get recognition {id}" });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RecognitionViewModel recognition)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var newRecognition = Mapper.Map<Recognition>(recognition);
                newRecognition.Sender = _repository.GetUserByEmail(recognition.SenderEmail);
                newRecognition.Receiver = _repository.GetUserByEmail(recognition.ReceiverEmail);
                newRecognition.Organization = _repository.GetOrganizationByName(recognition.OrganizationName);
                newRecognition.Value = _repository.GetCorporateValueByName(recognition.CorporateValueName);
                _repository.AddRecognition(newRecognition);
                if (await _repository.SaveChangesAsync()) return Created($"api/recognitions/{newRecognition.Id}", newRecognition);
            }
            catch (HighFiveException ex)
            {
                _logger.LogError("Failed to add new recognition: {0}", ex);
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add new recognition: {0}", ex);
            }
            return BadRequest(new { Message = $"Failed to add new recognition {recognition.Id}" });
        }
    }
}
