using AutoMapper;
using FluentAssertions;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.Controllers;
using HighFive.Server.Web.Utils;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace HighFive.Server.Specs.StepDefinitions.Authentication
{
    [Binding]
    public class LoginSteps
    {
        HighFiveUser _existingUser;
        UserViewModel _existingUserViewModel;
        Mock<IHighFiveRepository> _repo = new Mock<IHighFiveRepository>();
        AuthController _controller;
        IActionResult _result;

        public LoginSteps()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserViewModel, HighFiveUser>().ReverseMap();
            });
        }

        [Given]
        public void Given_The_following_user_exists(Table table)
        {
            _existingUser = table.CreateInstance<HighFiveUser>();
            _existingUserViewModel = Mapper.Map<UserViewModel>(_existingUser);
            _repo.Setup(r => r.GetUserByEmail(It.IsAny<string>())).Returns(_existingUser);

            var signInManager = new Mock<IWrapSignInManager<HighFiveUser>>();
            signInManager.Setup(m => m.PasswordSignInAsync("test.user@email.com", "password", It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));
            signInManager.Setup(m => m.PasswordSignInAsync("test.user@email.com", It.Is<string>(s => s != "password"), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));
            signInManager.Setup(m => m.PasswordSignInAsync(It.Is<string>(s => s != "test.user@email.com"), "password", It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));

            _controller = new AuthController(signInManager.Object, _repo.Object, _controllerLogger);
        }

        [When]
        public void I_login_with_the_following_information(Table table)
        {
            AuthViewModel auth = table.CreateInstance<AuthViewModel>();
            var result = _controller.Login(auth) as Task<IActionResult>;
            _result = result.Result;
        }

        [Then]
        public void Then_the_login_will_be_successful()
        {
            var okObjectResult = _result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            
        }

        [Then]
        public void Then_the_following_information_will_be_returned(Table table)
        {
            var okObjectResult = _result as OkObjectResult;
            okObjectResult.Value.ShouldBeEquivalentTo(
                _existingUserViewModel);
        }


        [Then]
        public void Then_the_login_will_be_unsuccessful()
        {
            _result.Should().BeOfType<UnauthorizedResult>();
        }

        #region properties

        private ILogger<AuthController> _controllerLogger => new Mock<ILogger<AuthController>>().Object;

        private ILogger<HighFiveRepository> _repoLogger => new Mock<ILogger<HighFiveRepository>>().Object;

        #endregion
    }
}
