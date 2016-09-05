#region references

using AutoMapper;
using FluentAssertions;
using HighFive.Server.Services.Models;
using HighFive.Server.Services.Utils;
using HighFive.Server.Web.Utils;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

#endregion

namespace HighFive.Server.Web.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        #region setup

        public AuthControllerTest()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserViewModel, HighFiveUser>().ReverseMap();
            });
        }

        #endregion

        #region tests

        [TestMethod]
        public void AuthController_Login_MissingEmail()
        {
            var signInManager = new Mock<IWrapSignInManager<HighFiveUser>>().Object;
            var controller = new AuthController(signInManager, _repository, _logger);
            controller.ViewData.ModelState.Clear();
            controller.ViewData.ModelState.AddModelError("Email", "The Email field is required.");
            controller.ViewData.ModelState.AddModelError("Password", "The Password field is required.");
            var user = new AuthViewModel();
            var result = controller.Login(user) as Task<IActionResult>;
            var viewresult = result.Result;
            viewresult.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public void AuthController_Login_FailedLogin()
        {
            var signInManager = new Mock<IWrapSignInManager<HighFiveUser>>();
            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));
            var controller = new AuthController(signInManager.Object, _repository, _logger);
            var user = new AuthViewModel { Email = "test.user@email.com", Password = "UhOhThisIsNotTheCorrectPassword" };
            var result = controller.Login(user) as Task<IActionResult>;
            var viewresult = result.Result;
            viewresult.Should().BeOfType<UnauthorizedResult>();
        }

        [TestMethod]
        public void AuthController_Login_UserNotFound()
        {
            var signInManager = new Mock<IWrapSignInManager<HighFiveUser>>();
            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));
            var repo = new Mock<IHighFiveRepository>();
            repo.Setup(r => r.GetUserByEmail(It.IsAny<string>())).Returns((HighFiveUser)null);
            var controller = new AuthController(signInManager.Object, repo.Object, _logger);
            var user = new AuthViewModel { Email = "test.user@email.com", Password = "password" };
            var result = controller.Login(user) as Task<IActionResult>;
            var viewresult = result.Result;
            viewresult.Should().BeOfType<NotFoundObjectResult>();
            var noFoundResult = viewresult as NotFoundObjectResult;
            AssertMessageProperty("User test.user@email.com not found", noFoundResult.Value);
        }

        [TestMethod]
        public void AuthController_Login_SimulatedServerFailure()
        {
            var signInManager = new Mock<IWrapSignInManager<HighFiveUser>>();
            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Throws<HighFiveException>();
            var repo = new Mock<IHighFiveRepository>();
            var controller = new AuthController(signInManager.Object, repo.Object, _logger);
            var result = controller.Login(new AuthViewModel { Email = "test.user@email.com", Password = "password" }) as Task<IActionResult>;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            AssertMessageProperty("Login Failed for user: test.user@email.com", badRequestResult.Value);
        }

        [TestMethod]
        public void AuthController_Logout_Successful()
        {
            var signInManager = new Mock<IWrapSignInManager<HighFiveUser>>();
            signInManager.Setup(m=>m.SignOutAsync()).Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));
            var repo = new Mock<IHighFiveRepository>();
            var controller = new AuthController(signInManager.Object, repo.Object, _logger);
            var result = controller.Delete() as Task<IActionResult>;
            var viewresult = result.Result;
            var okObjectResult = viewresult as OkObjectResult;
        }

        [TestMethod]
        public void AuthController_Logout_SimulatedServerFailure()
        {
            var signInManager = new Mock<IWrapSignInManager<HighFiveUser>>();
            signInManager.Setup(m => m.SignOutAsync()).Throws<HighFiveException>();
            var repo = new Mock<IHighFiveRepository>();
            var controller = new AuthController(signInManager.Object, repo.Object, _logger);
            var result = controller.Delete() as Task<IActionResult>;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            AssertMessageProperty("Failed to log user out.", badRequestResult.Value);
        }

        #endregion

        #region utilities

        private void AssertMessageProperty(string expectedMessage, object result)
        {
            object actualMessage = result.GetType().GetProperty("Message").GetValue(result, null);
            actualMessage.Should().Be(expectedMessage as string);
        }

        private ILogger<AuthController> _logger => new Mock<ILogger<AuthController>>().Object;

        private IHighFiveRepository _repository => new Mock<IHighFiveRepository>().Object;

        #endregion
    }
}

