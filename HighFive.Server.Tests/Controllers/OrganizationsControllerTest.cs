using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.ViewModels;
using HighFive.Server.Services.Utils;

namespace HighFive.Server.Web.Controllers
{
    [TestClass]
    public class OrganizationControllerTest : Controller
    {
        #region Constructor

        public OrganizationControllerTest()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<OrganizationViewModel, Organization>().ReverseMap();
            });
        }

        #endregion

        #region tests

        [TestMethod]
        public void OrganizationsControllers_GetAll_SunnyDay()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                context.Organizations.Add(new Organization { Name = "Ariel Partners", Values = new List<CorporateValue> { new CorporateValue { Name = "Test Name", Description = "Testing Description" } } });
                context.SaveChanges();

                var repo = new HighFiveRepository(context, _repoLogger);
                var controller = new OrganizationsController(repo, _controllerLogger);

                var result = controller.GetAll();
                result.Should().BeOfType<OkObjectResult>();
                var okResult = result as OkObjectResult;
                var orgList = okResult.Value as IList<OrganizationViewModel>;
                orgList.Should().HaveCount(1).And.ContainSingle(x => x.Name == "Ariel Partners");
            }
        }

        [TestMethod]
        public void OrganizationsControllers_GetAll_SimulatedServerFailure()
        {
            var repo = new Mock<IHighFiveRepository>();
            repo.Setup(r => r.GetAllOrganizations()).Throws<HighFiveException>();
            var controller = new OrganizationsController(repo.Object, _controllerLogger);

            var result = controller.GetAll();
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            AssertMessageProperty("Failed to get Organizations", badRequestResult.Value);
        }

        [TestMethod]
        public void OrganizationsControllers_GetOrganizationByName_SunnyDay()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                context.Organizations.Add(new Organization
                {
                    Name = "Ariel Partners",
                    Values = new List<CorporateValue>
                                    {
                                        new CorporateValue { Name="Commitment", Description="Committed to the long term success and happiness of our customers, our people, and our partners" },
                                        new CorporateValue { Name="Courage", Description="To take on difficult challenges, to accept new ideas, to accept incremental failure" },
                                        new CorporateValue { Name="Excellence", Description="Always strive to exceed expectations and continuously improve" },
                                        new CorporateValue { Name="Integrity", Description="Always act honestly, ethically, and do the right thing even when it hurts " }
                                    }
                });
                context.SaveChanges();

                var repo = new HighFiveRepository(context, _repoLogger);
                var controller = new OrganizationsController(repo, _controllerLogger);

                var result = controller.GetOrganizationByName("Ariel Partners");
                result.Should().BeOfType<OkObjectResult>();
                var okResult = result as OkObjectResult;
                var org = okResult.Value as OrganizationViewModel;
                org.Name.Should().Be("Ariel Partners");

                result = controller.GetOrganizationByName("Yeehaa");
                result.Should().BeOfType<NotFoundObjectResult>();
                var notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("Unable to find organization Yeehaa", notFoundResult.Value);
            }
        }

        [TestMethod]
        public void OrganizationsControllers_GetOrganizationByName_SimulatedServerFailure()
        {
            var repo = new Mock<IHighFiveRepository>();
            repo.Setup(r => r.GetOrganizationByName(It.IsAny<string>())).Throws<HighFiveException>();
            var controller = new OrganizationsController(repo.Object, _controllerLogger);

            var result = controller.GetOrganizationByName("Yeehaa");
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            AssertMessageProperty("Failed to get Organization Yeehaa", badRequestResult.Value);
        }

        [TestMethod]
        public void OrganizationsControllers_Post_SunnyDay()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new HighFiveRepository(context, _repoLogger);
                var controller = new OrganizationsController(repo, _controllerLogger);

                var org = controller.Post(new OrganizationViewModel { Name = "Macys", Values = new List<CorporateValue> { new CorporateValue { Name = "Test Name1", Description = "Testing Description1" } } });
                org.Result.Should().BeOfType<CreatedResult>();
                var createdResult = org.Result as CreatedResult;
                var organizationVm = createdResult.Value as OrganizationViewModel;
                organizationVm.Name.Should().Be("Macys");
            }
        }

        [TestMethod]
        public void OrganizationsControllers_Post_InvalidModel()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new HighFiveRepository(context, _repoLogger);
                var controller = new OrganizationsController(repo, _controllerLogger);

                controller.ViewData.ModelState.Clear();

                var noname = new OrganizationViewModel();
                controller.ViewData.ModelState.AddModelError("Name", "The Name field is required.");

                controller.ViewData.ModelState.Should().HaveCount(1);
                controller.ViewData.ModelState["Name"].Errors.Should().HaveCount(1);
                controller.ViewData.ModelState["Name"].Errors[0].ErrorMessage.Should().Be("The Name field is required.");

                var org = controller.Post(noname);
                org.Result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = org.Result as BadRequestObjectResult;
                var errLst = badRequestResult.Value as SerializableError;
                var errMsg = ((string[])errLst["Name"]).Aggregate(string.Empty, (current, errVal) => current + errVal);
                Assert.AreEqual("The Name field is required.", errMsg);
            }
        }

        [TestMethod]
        public void OrganizationsControllers_Post_DuplicateOrganization()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new HighFiveRepository(context, _repoLogger);
                var controller = new OrganizationsController(repo, _controllerLogger);
                //Adding an Organization
                var returnObject = controller.Post(new OrganizationViewModel { Name = "Macys", Values = new List<CorporateValue> { new CorporateValue { Name = "Test Name1", Description = "Testing Description1" } } });

                //Create an Organization with the same name
                returnObject = controller.Post(new OrganizationViewModel { Name = "Macys", Values = new List<CorporateValue> { new CorporateValue { Name = "Test Name2", Description = "Testing Description2" } } });
                returnObject.Result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = returnObject.Result as BadRequestObjectResult;
                AssertMessageProperty("Organization Macys already exists in the database", badRequestResult.Value);
            }
        }

        [TestMethod]
        public void OrganizationsControllers_Post_SimulatedServerFailure()
        {
            var repo = new Mock<IHighFiveRepository>();
            repo.Setup(r => r.GetOrganizationByName(It.IsAny<string>())).Throws<HighFiveException>();
            repo.Setup(r => r.AddOrganization(It.IsAny<Organization>())).Throws<HighFiveException>();
            var controller = new OrganizationsController(repo.Object, _controllerLogger);

            var returnObject = controller.Post(new OrganizationViewModel { Name = "Macys", Values = new List<CorporateValue> { new CorporateValue { Name = "Test Name1", Description = "Testing Description1" } } });
            returnObject.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = returnObject.Result as BadRequestObjectResult;
            AssertMessageProperty("Exception of type \'HighFive.Server.Services.Utils.HighFiveException\' was thrown.", badRequestResult.Value);
        }

        [TestMethod]
        public void OrganizationsControllers_Put_SunnyDay()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new HighFiveRepository(context, _repoLogger);
                var controller = new OrganizationsController(repo, _controllerLogger);
                //Adding an Organization
                controller.Post(new OrganizationViewModel { Name = "Macys", Values = new List<CorporateValue> { new CorporateValue { Name = "Test Name1", Description = "Testing Description1" } } });

                var returnObject = controller.Put("Macys", new OrganizationViewModel { Name = "Macys", Values = new List<CorporateValue> { new CorporateValue { Name = "Test Name1", Description = "Testing Description1" } } });
                returnObject.Result.Should().BeOfType<OkObjectResult>();
            }
        }

        #endregion

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

        private void AssertMessageProperty(string expectedMessage, object result)
        {
            var actualMessage = result.GetType().GetProperty("Message").GetValue(result, null);
            actualMessage.Should().Be(expectedMessage as string);
        }

        #region properties

        private IConfigurationRoot _config => new Mock<IConfigurationRoot>().Object;

        private ILogger<OrganizationsController> _controllerLogger => new Mock<ILogger<OrganizationsController>>().Object;

        private ILogger<HighFiveRepository> _repoLogger => new Mock<ILogger<HighFiveRepository>>().Object;

        #endregion
    }
}

