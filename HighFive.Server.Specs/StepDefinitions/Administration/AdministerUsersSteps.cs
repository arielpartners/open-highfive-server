
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HighFive.Server.Services.Models;

using TechTalk.SpecFlow;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using HighFive.Server.Web.Controllers;

namespace HighFive.Server.Specs.StepDefinitions.Administration
{
    [Binding]
    public class AdministerUsersSteps
    {
        
        [Given]
        public void Given_A_user_with_email_EMAIL_does_not_have_an_account(string email)
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                context.Organizations.Add(new Organization { Name = "Ariel Partners", Values = new List<CorporateValue> { new CorporateValue { Name = "Test Name", Description = "Testing Description" } } });
                context.SaveChanges();

                var repo = new HighFiveRepository(context, _repoLogger);
                var controller = new UsersController(repo, _controllerLogger);
            }
        }
        
        [Given]
        public void Given_A_user_exists(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When]
        public void When_I_create_an_user_account_for_them(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When]
        public void When_I_delete_the_user(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_an_account_should_be_created_for_them(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_they_can_login(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_The_user_is_removed_from_the_system(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_They_can_no_longer_login(Table table)
        {
            ScenarioContext.Current.Pending();
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

        private ILogger<UsersController> _controllerLogger => new Mock<ILogger<UsersController>>().Object;

        private ILogger<HighFiveRepository> _repoLogger => new Mock<ILogger<HighFiveRepository>>().Object;

        #endregion
    }
}
