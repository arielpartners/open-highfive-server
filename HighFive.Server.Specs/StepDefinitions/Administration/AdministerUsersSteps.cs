
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HighFive.Server.Services.Models;
using FluentAssertions;
using TechTalk.SpecFlow;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using HighFive.Server.Web.Controllers;
using HighFive.Server.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HighFive.Server.Specs.StepDefinitions.Administration
{
    [Binding]
    public class AdministerUsersSteps
    {
        private HighFiveRepository _reposCreateUser;
        private HighFiveRepository _reposDeleteUser;

        [Given]
        public void Given_A_user_with_email_EMAIL_does_not_have_an_account(string email)
        {
            //empty new context
            var options = CreateNewContextOptions();
            var context = new HighFiveContext(_config, options);
            context.Organizations.Add(new Organization { Name = "Ariel Partners", Values = new List<CorporateValue> { new CorporateValue { Name = "Test Name", Description = "Testing Description" } } });
            context.SaveChanges();
            _reposCreateUser = new HighFiveRepository(context, _repoLogger);
        }

        [Given]
        public void Given_A_user_with_username_EMAIL_and_password_PASSWORD_exists(string email, string password)
        {
            var options = CreateNewContextOptions();
            var context = new HighFiveContext(_config, options);
            context.Organizations.Add(new Organization { Name = "Ariel Partners", Values = new List<CorporateValue> { new CorporateValue { Name = "Test Name", Description = "Testing Description" } } });
            context.SaveChanges();
            _reposDeleteUser = new HighFiveRepository(context, _repoLogger);
            
            var signInManager = new Mock<IWrapSignInManager<HighFiveUser>>().Object;
            var controller = new AuthController(signInManager, _reposDeleteUser, _controllerLogger);
            // Following method needs to be implemented before SelfRegister can work
            // or wrap UserManager and add to services
            //var result = controller.SelfRegister(string username, string password, string organization)
        }

        [When]
        public void When_I_create_an_user_account_with_email_of_EMAIL_password_of_PASSWORD_and_Organization_of_ORGANIZATION(string email, string password, string organization)
        {
            var signInManager = new Mock<IWrapSignInManager<HighFiveUser>>().Object;
            var controller = new AuthController(signInManager, _reposCreateUser, _controllerLogger);
            // Following method needs to be implemented before SelfRegister can work
            //var result = controller.SelfRegister(string username, string password, string organization)
            ScenarioContext.Current.Pending();
        }

        [When]
        public void When_I_delete_the_user_with_username_EMAIL(string email)
        {
            var controller = new UsersController(_reposDeleteUser, _userControllerLogger);
            controller.Delete(email);
        }

        [Then]
        public void Then_an_account_should_be_created_for_them_with_a_username_EMAIL(string email)
        {
            var controller = new UsersController(_reposCreateUser, _userControllerLogger);
            var result = controller.GetByEmail(email);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Then]
        public void Then_The_user_with_username_EMAIL_is_removed_from_the_database(string email)
        {
            var controller = new UsersController(_reposDeleteUser, _userControllerLogger);
            var result = controller.GetByEmail(email);
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        #region utilities

        private static DbContextOptions<HighFiveContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<HighFiveContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        #endregion

        #region properties

        private IConfigurationRoot _config => new Mock<IConfigurationRoot>().Object;

        private ILogger<AuthController> _controllerLogger => new Mock<ILogger<AuthController>>().Object;
        private ILogger<UsersController> _userControllerLogger => new Mock<ILogger<UsersController>>().Object;

        private ILogger<HighFiveRepository> _repoLogger => new Mock<ILogger<HighFiveRepository>>().Object;

        #endregion
    }
}
