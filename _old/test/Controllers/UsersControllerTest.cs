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
        public void TestGetAll_SunnyDay()
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
        }

        [Fact]
        public void TestGetAll_SimulatedServerFailure()
        {
            var options = CreateNewContextOptions();
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
        public void TestGetByEmail_SunnyDay()
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
        public void TestGetByEmail_SimulatedServerFailure()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new Mock<IHighFiveRepository>();
                repo.Setup(r => r.GetUserByEmail(It.IsAny<String>())).Throws<Exception>();
                UsersController controller = new UsersController(repo.Object, _controllerLogger);

                var result = controller.GetByEmail("i@dontexist.com");
                Assert.IsType(typeof(BadRequestObjectResult), result);
                var badRequestResult = result as BadRequestObjectResult;
                AssertMessageProperty("Failed to get user", badRequestResult.Value);
            }
        }

        [Fact]
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
                UsersController controller = new UsersController(repo, _controllerLogger);

                var newUser = new UserViewModel() { Email = "c@d.com", OrganizationName = "Ariel Partners" };
                var result = controller.Post(newUser);
                Assert.IsType(typeof(CreatedResult), result);
                var createdResult = result as CreatedResult;
                var user = createdResult.Value as UserViewModel;
                Assert.Equal("c@d.com", user.Email);
            }
        }

        [Fact]
        public void TestPost_DuplicateUser()
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

                var duplicateUser = new UserViewModel() { Email = "a@b.com", OrganizationName = "Ariel Partners" };
                var result2 = controller.Post(duplicateUser);
                Assert.IsType(typeof(BadRequestObjectResult), result2);
                var badRequestResult = result2 as BadRequestObjectResult;
                AssertMessageProperty("Failed to add new user a@b.com", badRequestResult.Value);

            }
        }

        [Fact]
        public void TestPost_UnknownOrganization()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                UsersController controller = new UsersController(repo, _controllerLogger);
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
        public void TestPost_SimulatedServerFailure()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var organization = new Organization() { Name = "Ariel Partners" };
                context.Organizations.Add(organization);
                context.Users.Add(new HighFiveUser() { Email = "a@b.com", Organization = organization });
                context.SaveChanges();
           
                var repo = new Mock<IHighFiveRepository>();
                repo.Setup(r => r.GetOrganizationByName(It.IsAny<String>())).Returns(organization);
                repo.Setup(r => r.AddUser(It.IsAny<HighFiveUser>())).Throws<Exception>();
                UsersController controller = new UsersController(repo.Object, _controllerLogger);

                var newUser = new UserViewModel() { Email = "c@d.com", OrganizationName = "Ariel Partners" };
                var result = controller.Post(newUser);
                Assert.IsType(typeof(BadRequestObjectResult), result);
                var badRequestResult = result as BadRequestObjectResult;
                AssertMessageProperty("Failed to add new user c@d.com", badRequestResult.Value);
            }
        }

        [Fact]
        public void TestDelete_SunnyDay()
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
        public void TestDelete_SimulatedServerFailure()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var organization = new Organization() { Name = "Ariel Partners" };
                context.Organizations.Add(organization);
                var user = new HighFiveUser() { Email = "a@b.com", Organization = organization };
                context.Users.Add(user);
                context.SaveChanges();
           
                var repo = new Mock<IHighFiveRepository>();
                repo.Setup(r => r.GetUserByEmail(It.IsAny<String>())).Returns(user);
                repo.Setup(r => r.DeleteUser(It.IsAny<HighFiveUser>())).Throws<Exception>();
                UsersController controller = new UsersController(repo.Object, _controllerLogger);

                var result = controller.Delete("a@b.com");
                Assert.IsType(typeof(BadRequestObjectResult), result);
                var badRequestResult = result as BadRequestObjectResult;
                AssertMessageProperty("Failed to delete user a@b.com", badRequestResult.Value);
            }
        }

        [Fact]
        public void TestPut_UserNotFound()
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
            }
        }

        [Fact]
        public void TestPut_SunnyDay()
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
                var result = controller.Put("a@b.com", updatedUser);
                Assert.IsType(typeof(OkObjectResult), result);
                var okObjectResult = result as OkObjectResult;
                AssertMessageProperty("User c@d.com updated successfully", okObjectResult.Value);

                updatedUser.OrganizationName = "Acme";
                result = controller.Put("c@d.com", updatedUser);
                Assert.IsType(typeof(OkObjectResult), result);
                okObjectResult = result as OkObjectResult;
                AssertMessageProperty("User c@d.com updated successfully", okObjectResult.Value);
            }
        }

        [Fact]
        public void TestPut_NoChange()
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

                var updatedUser = new UserViewModel() { Email = "a@b.com", OrganizationName = "Ariel Partners" };
                var result = controller.Put("a@b.com", updatedUser);
                Assert.IsType(typeof(OkObjectResult), result);
                var okObjectResult = result as OkObjectResult;
                AssertMessageProperty("User a@b.com was not changed", okObjectResult.Value);
            }
        }

        [Fact]
        public void TestPut_OrganizationNotFound()
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

                var updatedUser = new UserViewModel() { Email = "a@b.com", OrganizationName = "IDontExist" };
                var result = controller.Put("a@b.com", updatedUser);
                Assert.IsType(typeof(NotFoundObjectResult), result);
                var notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("Organization IDontExist not found", notFoundResult.Value);
            }
        }

        [Fact]
        public void TestPut_SimulateServerFailure()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var organization = new Organization() { Name = "Ariel Partners" };
                context.Organizations.Add(organization);
                var organization2 = new Organization() { Name = "Acme" };
                context.Organizations.Add(organization2);
                var user = new HighFiveUser() { Email = "a@b.com", Organization = organization };
                context.Users.Add(user);
                context.SaveChanges();
           
                var repo = new Mock<IHighFiveRepository>();
                UsersController controller = new UsersController(repo.Object, _controllerLogger);
                repo.Setup(r => r.GetUserByEmail(It.IsAny<String>())).Returns(user);
                repo.Setup(r => r.UpdateUser(It.IsAny<HighFiveUser>())).Throws<Exception>();

                var updatedUser = new UserViewModel() { Email = "a@b.com", OrganizationName = "IDontExist" };
                var result = controller.Put("a@b.com", updatedUser);
                Assert.IsType(typeof(NotFoundObjectResult), result);
                var notFoundResult = result as NotFoundObjectResult;
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
