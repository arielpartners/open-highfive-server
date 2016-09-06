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
                if (recognitions.Any()) return Ok(Mapper.Map<List<RecognitionViewModel>>(recognitions));
                return NoContent();
            }
            catch (DbException ex)
            {
                _logger.LogError("Failed to get recognitions: {0}", ex);
            }
            return BadRequest(new { Message = "Failed to get recognitions" });
        }

        //[HttpGet("{id}")]
        //public IActionResult GetRecognitionById(int id)
        //{
        //    try
        //    {
        //        var recognition = _repository.GetRecognitionById(id);
        //        if (recognition!=null) return Ok(Mapper.Map<RecognitionViewModel>(recognition));
        //        return NoContent();
        //    }
        //    catch (DbException ex)
        //    {
        //        _logger.LogError("Failed to get recognition: {0}", ex);
        //    }
        //    return BadRequest(new { Message = $"Failed to get recognition {id}" });
        //}

        [HttpGet("{receiverName}")]
        public IActionResult GetRecognitionsByReceiver(string receiverName)
        {
            try
            {
                var recognitions = _repository.GetRecognitionsByReceiver(receiverName);
                if (recognitions.Any()) return Ok(Mapper.Map<List<RecognitionViewModel>>(recognitions));
                return NoContent();
            }
            catch (DbException ex)
            {
                _logger.LogError("Failed to get recognitions by receiver: {0}", ex);
            }
            return BadRequest(new { Message = $"Failed to get recognitions by receiver {receiverName}" });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RecognitionViewModel recognition)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrEmpty(recognition.SenderEmail)) return BadRequest(new { Message = "Missing Sender Information" });
            if (string.IsNullOrEmpty(recognition.OrganizationName)) return BadRequest(new { Message = "Missing Organization Information" });
            if (string.IsNullOrEmpty(recognition.CorporateValueName)) return BadRequest(new { Message = "Missing Corporate Value Information" });
            try
            {
                var newRecognition = Mapper.Map<Recognition>(recognition);
                newRecognition.Organization = _repository.GetOrganizationByName(recognition.OrganizationName);

                if (!String.IsNullOrEmpty(recognition.NewUserEmail))
                {
                    // we need to make a new user
                    var newUser = _repository.GetUserByEmail(recognition.NewUserEmail);
                    if (newUser == null)
                    {
                        var theNewUser = new HighFiveUser()
                        {
                            Email = recognition.NewUserEmail,
                            FirstName = HighFiveUser.FirstNameFromName(recognition.NewUserName),
                            LastName = HighFiveUser.LastNameFromName(recognition.NewUserName),
                            Organization = newRecognition.Organization
                        };
                        _repository.AddUser(theNewUser);
                        await _repository.SaveChangesAsync();
                    }
                    newRecognition.Receiver = _repository.GetUserByEmail(recognition.NewUserEmail);
                }
                else
                {
                    newRecognition.Receiver = _repository.GetUserByEmail(recognition.ReceiverEmail);
                }
                
                newRecognition.Sender = _repository.GetUserByEmail(recognition.SenderEmail);
                newRecognition.Value = _repository.GetCorporateValueByName(recognition.CorporateValueName);
                if(newRecognition.Value==null) return BadRequest(new { Message = "Missing Corporate Value Information" });
                newRecognition.DateCreated = DateTime.UtcNow;
                _repository.AddRecognition(newRecognition);
                if (await _repository.SaveChangesAsync()) return Created($"api/recognitions/{newRecognition.Id}", Mapper.Map<RecognitionViewModel>(newRecognition));
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
