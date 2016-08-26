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
    public class UsersControllerTest
    {
        #region Constructor

        public UsersControllerTest()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserViewModel, HighFiveUser>().ReverseMap();
            });
        }

        #endregion

        #region TestGetAll

        [Fact]
        public void TestGetAll()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                context.Users.Add(new HighFiveUser { Email = "a@b.com" });
                context.SaveChanges();
            }

            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                UsersController controller = new UsersController(repo, _controllerLogger);

                var result = controller.GetAll();
                Assert.IsType(typeof(OkObjectResult), result);
                var okResult = result as OkObjectResult;
                var userList = okResult.Value as IList<HighFiveUser>;
                Assert.Equal(1, userList.Count());
                Assert.Equal("a@b.com", userList[0].Email);
            }

            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new Mock<IHighFiveRepository>();
                repo.Setup(r => r.GetAllUsers()).Throws<Exception>();
                UsersController controller = new UsersController(repo.Object, _controllerLogger);

                var result = controller.GetAll();
                Assert.IsType(typeof(BadRequestObjectResult), result);
                var badRequestResult = result as BadRequestObjectResult;
                AssertMessageProperty("Failed to get users", badRequestResult.Value);
            }
        }

        #endregion

        #region TestGetByEmail

        [Fact]
        public void TestGetByEmail()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                context.Users.Add(new HighFiveUser { Email = "a@b.com" });
                context.SaveChanges();
            }

            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                UsersController controller = new UsersController(repo, _controllerLogger);

                var result = controller.GetByEmail("a@b.com");
                Assert.IsType(typeof(OkObjectResult), result);
                var okResult = result as OkObjectResult;
                var user = okResult.Value as HighFiveUser;
                Assert.Equal("a@b.com", user.Email);

                result = controller.GetByEmail("i@dontexist.com");
                Assert.IsType(typeof(NotFoundObjectResult), result);
                var notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("User i@dontexist.com not found", notFoundResult.Value);
            }
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
                UsersController controller = new UsersController(repo, _controllerLogger);

                var newUser = new UserViewModel() { Email = "c@d.com", OrganizationName = "Ariel Partners" };
                var result = controller.Post(newUser);
                Assert.IsType(typeof(CreatedResult), result);
                var createdResult = result as CreatedResult;
                var user = createdResult.Value as UserViewModel;
                Assert.Equal("c@d.com", user.Email);

                var duplicateUser = new UserViewModel() { Email = "c@d.com", OrganizationName = "Ariel Partners" };
                var result2 = controller.Post(duplicateUser);
                Assert.IsType(typeof(BadRequestObjectResult), result2);
                var badRequestResult = result2 as BadRequestObjectResult;
                AssertMessageProperty("Failed to add new user c@d.com", badRequestResult.Value);

                var unknownOrgUser = new UserViewModel() { Email = "zip@zap.com", OrganizationName = "Bad Guys" };
                var result3 = controller.Post(unknownOrgUser);
                Assert.IsType(typeof(NotFoundObjectResult), result3);
                var notFoundResult = result3 as NotFoundObjectResult;
                AssertMessageProperty("Unable to find organization Bad Guys", notFoundResult.Value);
            }
        }

        #endregion

        #region TestDelete

        [Fact]
        public void TestDelete()
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
                UsersController controller = new UsersController(repo, _controllerLogger);

                var result = controller.Delete("i@dontexist.com");
                Assert.IsType(typeof(NotFoundObjectResult), result);
                var notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("User i@dontexist.com not found", notFoundResult.Value);
                Assert.Equal(1, context.Users.Count());

                result = controller.Delete("a@b.com");
                Assert.IsType(typeof(OkObjectResult), result);
                Assert.Equal(0, context.Users.Count());

                result = controller.Delete("a@b.com");
                Assert.IsType(typeof(NotFoundObjectResult), result);
                notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("User a@b.com not found", notFoundResult.Value);
            }
        }

        #endregion

        #region TestPut

        [Fact]
        public void TestPut()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var organization = new Organization() { Name = "Ariel Partners" };
                context.Organizations.Add(organization);
                var organization2 = new Organization() { Name = "Acme" };
                context.Organizations.Add(organization2);
                context.Users.Add(new HighFiveUser() { Email = "a@b.com", Organization = organization });
                context.SaveChanges();
            }

            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                UsersController controller = new UsersController(repo, _controllerLogger);

                var updatedUser = new UserViewModel() { Email = "c@d.com", OrganizationName = "Ariel Partners" };
                var result = controller.Put("c@d.com", updatedUser);
                Assert.IsType(typeof(NotFoundObjectResult), result);
                var notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("User c@d.com not found", notFoundResult.Value);

                updatedUser.Email = "c@e.com";
                result = controller.Put("a@b.com", updatedUser);
                Assert.IsType(typeof(OkObjectResult), result);
                var okObjectResult = result as OkObjectResult;
                AssertMessageProperty("User c@e.com updated successfully", okObjectResult.Value);

                updatedUser.OrganizationName = "Acme" ;
                result = controller.Put("c@e.com", updatedUser);
                Assert.IsType(typeof(OkObjectResult), result);
                okObjectResult = result as OkObjectResult;
                AssertMessageProperty("User c@e.com updated successfully", okObjectResult.Value);

                result = controller.Put("c@e.com", updatedUser);
                Assert.IsType(typeof(OkObjectResult), result);
                okObjectResult = result as OkObjectResult;
                AssertMessageProperty("User c@e.com was not changed", okObjectResult.Value);

                updatedUser.OrganizationName = "IDontExist";
                result = controller.Put("c@e.com", updatedUser);
                Assert.IsType(typeof(NotFoundObjectResult), result);
                notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("Organization IDontExist not found", notFoundResult.Value);
            }
        }

        #endregion

        #region AssertMessageProperty

        private void AssertMessageProperty(string expectedMessage, object result)
        {
            object actualMessage = result.GetType().GetProperty("Message").GetValue(result, null);
            Assert.Equal(expectedMessage, actualMessage as string);
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

        private ILogger<UsersController> _controllerLogger
        {
            get
            {
                return new Mock<ILogger<UsersController>>().Object;
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
