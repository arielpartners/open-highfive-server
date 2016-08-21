using AutoMapper;
using highfive_server.Models;
using highfive_server.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace highfive_server.Controllers
{
    public class UsersControllerTest
    {
        public UsersControllerTest()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserViewModel, HighFiveUser>().ReverseMap();
                //config.CreateMap<UserViewModel, Organization>().ReverseMap();
            });
        }

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
        }

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
            }
        }

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

                var unknownOrgUser = new UserViewModel() { Email = "zip@zap.com", OrganizationName = "Bad Guys" };
                var result3 = controller.Post(unknownOrgUser);
                Assert.IsType(typeof(NotFoundObjectResult), result3);
            }
        }

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
    }
}
