#region references

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using HighFive.Server.Services.Models;
using HighFive.Server.Web.ViewModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;

#endregion

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

        // POST api/values
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthViewModel model)
        {
            if (model == null) return BadRequest(new { Message = "User/Pwd information missing" });
            try
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Pwd, true, false);
                if (!signInResult.Succeeded) return NotFound(new {Message = $"Invaild User/Pwd {model.Email}"});
                var usr = _repository.GetUserByEmail(model.Email);
                if (usr == null) return NotFound(new { Message = $"User not found {model.Email}" });
                return Ok(usr);
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
        public async Task<IActionResult> Delete()
        {
            try
            {
                await _signInManager.SignOutAsync();
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
