#region references

using AutoMapper;
using FluentAssertions;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.Utils;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

#endregion

namespace HighFive.Server.Web.Controllers
{
    [TestClass]
    public class AuthControllerTest : Controller
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
            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<bool>(), It.IsAny<bool>()))
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
            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));
            var repo = new Mock<IHighFiveRepository>();
            repo.Setup(r => r.GetUserByEmail(It.IsAny<String>())).Returns((HighFiveUser)null);
            var controller = new AuthController(signInManager.Object, repo.Object, _logger);
            var user = new AuthViewModel { Email = "test.user@email.com", Password = "password" };
            var result = controller.Login(user) as Task<IActionResult>;
            var viewresult = result.Result;
            viewresult.Should().BeOfType<NotFoundObjectResult>();
            var noFoundResult = viewresult as NotFoundObjectResult;
            AssertMessageProperty("User test.user@email.com not found", noFoundResult.Value);
        }

        [TestMethod]
        public void AuthController_Login_Successful()
        {
            var signInManager = new Mock<IWrapSignInManager<HighFiveUser>>();
            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));
            var repo = new Mock<IHighFiveRepository>();
            var user = new HighFiveUser() { Email = "test.user@email.com" };
            repo.Setup(r => r.GetUserByEmail(It.IsAny<String>())).Returns(user);
            var controller = new AuthController(signInManager.Object, repo.Object, _logger);
            var authUser = new AuthViewModel { Email = "test.user@email.com", Password = "password" };
            var result = controller.Login(authUser) as Task<IActionResult>;
            var viewresult = result.Result;
            var okObjectResult = viewresult as OkObjectResult;
            okObjectResult.Value.ShouldBeEquivalentTo(
                new UserViewModel() { Email = "test.user@email.com" }, options => options
                .Excluding(ctx => ctx.SelectedMemberPath == "DateCreated"));
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

