using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using HighFive.Server.Services.Models;
using HighFive.Server.ViewModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HighFive.Server.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private IHighFiveRepository _repository;
        private ILogger<AuthController> _logger;
        private SignInManager<HighFiveUser> _signInManager;

        public AuthController(SignInManager<HighFiveUser> signInManager, IHighFiveRepository repository, ILogger<AuthController> logger)
        {
            _repository = repository;
            _logger = logger;
            _signInManager = signInManager;
        }

        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthViewModel model)
        {
            if (model == null) return BadRequest(new { Message = $"User/Pwd information missing" });
            try
            {
                //var user = AutoMapper.Mapper.Map<HighFiveUser>(model);
                var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                if (signInResult.Succeeded)
                {
                    var usr = _repository.GetUserByEmail(model.Email);
                    if (usr == null) return NotFound(new { Message = $"User not found {model.Email}" });
                    return Ok(usr);
                }
                else
                {
                    return NotFound(new { Message = $"Invaild User/Pwd {model.Email}" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Login Failed for user: {0} {1}", model.Email, ex);
            }
            return BadRequest(new { Message = "Failed to get user" });
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/values/5
        [HttpDelete()]
        public IActionResult Delete()
        {
            try
            {
                _signInManager.SignOutAsync();
            }
            catch(Exception e)
            {
                _logger.LogCritical("Excpetion trying to log user out", e.StackTrace);
                return BadRequest(new { Message = "Failed to log user out." });
            }
            return Ok(null);
        }
    }
}
