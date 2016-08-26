#region project references

using AutoMapper;
using highfive_server.Models;
using highfive_server.ViewModels;
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
using Xunit;

#endregion

namespace highfive_server.Controllers
{
    [Route("api/[controller]")]
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

        [Fact]
        public void TestPost()
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
                Assert.IsType(typeof(CreatedResult), result);
                var createdResult = result as CreatedResult;
                var organization = createdResult.Value as OrganizationViewModel;
                Assert.Equal("Macys", organization.Name);
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

