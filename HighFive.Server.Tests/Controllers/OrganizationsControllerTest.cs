#region references

using AutoMapper;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

#endregion

namespace HighFive.Server.Web.Controllers
{
    [TestClass]
    public class OrganizationsControllerTest : Controller
    {
        //private IHighFiveRepository _repository;
        //private ILogger<OrganizationsController> _logger;

        #region Constructor

        public OrganizationsControllerTest()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<OrganizationViewModel, Organization>().ReverseMap();
            });
        }

        #endregion
        
        #region TestPost

        [TestMethod]
        public void TestPost_SunnyDay()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var organization = new Organization() { Name = "Ariel Partners" };
                context.Organizations.Add(organization);
                context.Users.Add(new HighFiveUser() { Email = "a@b.com", Organization = organization });
                context.SaveChanges();
            }

            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                OrganizationsController controller = new OrganizationsController(repo, _controllerLogger);

                var newOrganization = new OrganizationViewModel() { Name = "Macys"};
                var result = controller.Post(newOrganization);
                result.Should().BeOfType<CreatedResult>();
                var createdResult = result as CreatedResult;
                var organization = createdResult.Value as OrganizationViewModel;
                organization.Name.Should().Be("Macys");
            }
        }

        [TestMethod]
        public void TestPost_InvalidModel()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var organization = new Organization() { Name = "Ariel Partners" };
                context.Organizations.Add(organization);
                context.Users.Add(new HighFiveUser() { Email = "a@b.com", Organization = organization });
                context.SaveChanges();
            }

            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                OrganizationsController controller = new OrganizationsController(repo, _controllerLogger);

                controller.ViewData.ModelState.Clear();
                
                var noname = new OrganizationViewModel();
                controller.ViewData.ModelState.AddModelError("Name", "The Name field is required.");

                controller.ViewData.ModelState.Should().HaveCount(1);
                controller.ViewData.ModelState["Name"].Errors.Should().HaveCount(1);
                controller.ViewData.ModelState["Name"].Errors[0].ErrorMessage.Should().Be("The Name field is required.");

                var result = controller.Post(noname);
                result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result as BadRequestObjectResult;
            }
        }

        #endregion

        #region DbContextOptions

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

        private void AssertMessageProperty(string expectedMessage, object result)
        {
            object actualMessage = result.GetType().GetProperty("Message").GetValue(result, null);
            expectedMessage.Should().Be(actualMessage as string);
        }

        #region properties

        private IConfigurationRoot _config
        {
            get
            {
                return new Mock<IConfigurationRoot>().Object;
            }
        }

        private ILogger<OrganizationsController> _controllerLogger
        {
            get
            {
                return new Mock<ILogger<OrganizationsController>>().Object;
            }
        }

        private ILogger<HighFiveRepository> _repoLogger
        {
            get
            {
                return new Mock<ILogger<HighFiveRepository>>().Object;
            }
        }

        #endregion
    }
}

