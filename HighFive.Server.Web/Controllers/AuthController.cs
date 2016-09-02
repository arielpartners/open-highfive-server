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

#endregion

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HighFive.Server.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IHighFiveRepository _repository;
        private readonly ILogger<AuthController> _logger;
        private readonly SignInManager<HighFiveUser> _signInManager;

        #region Constructor

        public AuthController(SignInManager<HighFiveUser> signInManager, IHighFiveRepository repository, ILogger<AuthController> logger)
        {
            _repository = repository;
            _logger = logger;
            _signInManager = signInManager;
        }

        #endregion

        #region Login - POST api/values

        // POST api/values
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model == null) return BadRequest(new { Message = "User/Password information missing" });
            try
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                if (!signInResult.Succeeded) return NotFound(new {Message = $"Invalid User/Password {model.Email}"});
                var usr = _repository.GetUserByEmail(model.Email);
                if (usr == null) return NotFound(new { Message = $"User not found {model.Email}" });
                return Ok(Mapper.Map<UserViewModel>(usr));
            }
            catch (HighFiveException ex)
            {
                _logger.LogError("Login Failed for user: {0} {1}", model.Email, ex);
            }
            return BadRequest(new { Message = "Failed to get user" });
        }

        #endregion

        #region Delete - DELETE api/values/5

        // DELETE api/values/5
        [HttpDelete()]
        public async Task<IActionResult> Delete()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch(HighFiveException e)
            {
                _logger.LogCritical("Excpetion trying to log user out", e.StackTrace);
                return BadRequest(new { Message = "Failed to log user out." });
            }
            return Ok(null);
        }

        #endregion
    }
}
