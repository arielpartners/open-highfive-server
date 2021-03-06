﻿#region references

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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using HighFive.Server.Services.Utils;

#endregion

namespace HighFive.Server.Web.Controllers
{
    [TestClass]
    public class UsersControllerTest
    {
        #region setup

        public UsersControllerTest()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserViewModel, HighFiveUser>().ReverseMap();
                config.CreateMap<HighFiveUser, UserViewModel>()
                    .ForMember(g => g.OrganizationName, opt => opt.MapFrom(u => u.Organization.Name))
                    .ForMember(g => g.OrganizationWebPath, opt => opt.MapFrom(u => u.Organization.WebPath));
            });
        }

        #endregion

        #region tests

        [TestMethod]
        public void UsersController_GetAll_SunnyDay()
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
                result.Should().BeOfType<OkObjectResult>();
                var okResult = result as OkObjectResult;
                var userList = okResult.Value as IList<UserViewModel>;
                userList.Should()
                    .HaveCount(1)
                    .And.ContainSingle(x => x.Email == "a@b.com");
            }
        }

        [TestMethod]
        public void UsersController_GetAll_SimulatedServerFailure()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new Mock<IHighFiveRepository>();
                repo.Setup(r => r.GetAllUsers()).Throws<HighFiveException>();
                UsersController controller = new UsersController(repo.Object, _controllerLogger);

                var result = controller.GetAll();
                result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result as BadRequestObjectResult;
                AssertMessageProperty("Failed to get users", badRequestResult.Value);
            }
        }

        [TestMethod]
        public void UsersController_GetAll_NoContent()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new HighFiveRepository(context, _repoLogger);
                var controller = new UsersController(repo, _controllerLogger);

                var result = controller.GetAll();
                result.Should().BeOfType<NoContentResult>();
            }
        }

        [TestMethod]
        public void UsersController_GetByEmail_SunnyDay()
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
                result.Should().BeOfType<OkObjectResult>();
                var okResult = result as OkObjectResult;
                var user = okResult.Value as UserViewModel;
                user.Email.Should().Be("a@b.com");

                result = controller.GetByEmail("i@dontexist.com");
                result.Should().BeOfType<NotFoundObjectResult>();
                var notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("User i@dontexist.com not found", notFoundResult.Value);
            }
        }


        [TestMethod]
        public void UsersController_GetByEmail_SimulatedServerFailure()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new Mock<IHighFiveRepository>();
                repo.Setup(r => r.GetUserByEmail(It.IsAny<String>())).Throws<HighFiveException>();
                UsersController controller = new UsersController(repo.Object, _controllerLogger);

                var result = controller.GetByEmail("i@dontexist.com");
                result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result as BadRequestObjectResult;
                AssertMessageProperty("Failed to get user", badRequestResult.Value);
            }
        }

        [TestMethod]
        public void UsersController_Post_SunnyDay()
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
                result.Should().BeOfType<CreatedResult>();
                var createdResult = result as CreatedResult;
                var user = createdResult.Value as UserViewModel;
                user.Email.Should().Be("c@d.com");
            }
        }

        [TestMethod]
        public void UsersController_Post_DuplicateUser()
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
                var result = controller.Post(duplicateUser);
                result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result as BadRequestObjectResult;
                AssertMessageProperty("Failed to add new user a@b.com", badRequestResult.Value);

            }
        }

        [TestMethod]
        public void UsersController_Post_UnknownOrganization()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                UsersController controller = new UsersController(repo, _controllerLogger);
                var unknownOrgUser = new UserViewModel() { Email = "zip@zap.com", OrganizationName = "Bad Guys" };
                var result = controller.Post(unknownOrgUser);
                result.Should().BeOfType<NotFoundObjectResult>();
                var notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("Unable to find organization Bad Guys", notFoundResult.Value);
            }
        }

        [TestMethod]
        public void UsersController_Post_SimulatedServerFailure()
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
                repo.Setup(r => r.AddUser(It.IsAny<HighFiveUser>())).Throws<HighFiveException>();
                UsersController controller = new UsersController(repo.Object, _controllerLogger);

                var newUser = new UserViewModel() { Email = "c@d.com", OrganizationName = "Ariel Partners" };
                var result = controller.Post(newUser);
                result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result as BadRequestObjectResult;
                AssertMessageProperty("Failed to add new user c@d.com", badRequestResult.Value);
            }
        }

        [TestMethod]
        public void UsersController_Delete_SunnyDay()
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
                result.Should().BeOfType<NotFoundObjectResult>();
                var notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("User i@dontexist.com not found", notFoundResult.Value);
                context.Users.Should().HaveCount(1);

                result = controller.Delete("a@b.com");
                result.Should().BeOfType<OkObjectResult>();
               
                context.Users.Should().BeEmpty();

                result = controller.Delete("a@b.com");
                result.Should().BeOfType<NotFoundObjectResult>();
                notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("User a@b.com not found", notFoundResult.Value);
            }
        }

        [TestMethod]
        public void UsersController_Delete_SimulatedServerFailure()
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
                repo.Setup(r => r.DeleteUser(It.IsAny<HighFiveUser>())).Throws<HighFiveException>();
                UsersController controller = new UsersController(repo.Object, _controllerLogger);

                var result = controller.Delete("a@b.com");
                result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result as BadRequestObjectResult;
                AssertMessageProperty("Failed to delete user a@b.com", badRequestResult.Value);
            }
        }

        [TestMethod]
        public void UsersController_Put_UserNotFound()
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
                result.Should().BeOfType<NotFoundObjectResult>();
                var notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("User c@d.com not found", notFoundResult.Value);
            }
        }

        [TestMethod]
        public void UsersController_Put_SunnyDay()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var organization = new Organization() { Name = "Ariel Partners",
                    Values = new List<CorporateValue>
                                    {
                                        new CorporateValue { Name="Commitment", Description="Committed to the long term success and happiness of our customers, our people, and our partners" },
                                        new CorporateValue { Name="Courage", Description="To take on difficult challenges, to accept new ideas, to accept incremental failure" },
                                        new CorporateValue { Name="Excellence", Description="Always strive to exceed expectations and continuously improve" },
                                        new CorporateValue { Name="Integrity", Description="Always act honestly, ethically, and do the right thing even when it hurts " }
                                    }
                };
                context.Organizations.Add(organization);
                var organization2 = new Organization() { Name = "Acme",
                    Values = new List<CorporateValue>
                                    {
                                        new CorporateValue { Name="Commitment", Description="Committed to the long term success and happiness of our customers, our people, and our partners" },
                                        new CorporateValue { Name="Courage", Description="To take on difficult challenges, to accept new ideas, to accept incremental failure" },
                                        new CorporateValue { Name="Excellence", Description="Always strive to exceed expectations and continuously improve" },
                                        new CorporateValue { Name="Integrity", Description="Always act honestly, ethically, and do the right thing even when it hurts " }
                                    }
                };
                context.Organizations.Add(organization2);
                context.Users.Add(new HighFiveUser() { Email = "a@b.com", Organization = organization });
                context.SaveChanges();
            
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                UsersController controller = new UsersController(repo, _controllerLogger);

                var updatedUser = new UserViewModel() { Email = "c@d.com", OrganizationName = "Ariel Partners" };
                var result = controller.Put("a@b.com", updatedUser);
                result.Should().BeOfType<OkObjectResult>();
                var okObjectResult = result as OkObjectResult;
                AssertMessageProperty("User c@d.com updated successfully", okObjectResult.Value);

                //updatedUser.OrganizationName = "Acme";
                //result = controller.Put("c@d.com", updatedUser);
                //result.Should().BeOfType<OkObjectResult>();
                //okObjectResult = result as OkObjectResult;
                //AssertMessageProperty("User c@d.com updated successfully", okObjectResult.Value);
            }
        }

        [TestMethod]
        public void UsersController_Put_NoChange()
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
                result.Should().BeOfType<OkObjectResult>();
                var okObjectResult = result as OkObjectResult;
                AssertMessageProperty("User a@b.com was not changed", okObjectResult.Value);
            }
        }

        [TestMethod]
        public void UsersController_Put_OrganizationNotFound()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var organization = new Organization() { Name = "Ariel Partners",
                    Values = new List<CorporateValue>
                                    {
                                        new CorporateValue { Name="Commitment", Description="Committed to the long term success and happiness of our customers, our people, and our partners" },
                                        new CorporateValue { Name="Courage", Description="To take on difficult challenges, to accept new ideas, to accept incremental failure" },
                                        new CorporateValue { Name="Excellence", Description="Always strive to exceed expectations and continuously improve" },
                                        new CorporateValue { Name="Integrity", Description="Always act honestly, ethically, and do the right thing even when it hurts " }
                                    }
                };
                context.Organizations.Add(organization);
                var organization2 = new Organization() { Name = "Acme",
                    Values = new List<CorporateValue>
                                    {
                                        new CorporateValue { Name="Commitment", Description="Committed to the long term success and happiness of our customers, our people, and our partners" },
                                        new CorporateValue { Name="Courage", Description="To take on difficult challenges, to accept new ideas, to accept incremental failure" },
                                        new CorporateValue { Name="Excellence", Description="Always strive to exceed expectations and continuously improve" },
                                        new CorporateValue { Name="Integrity", Description="Always act honestly, ethically, and do the right thing even when it hurts " }
                                    }
                };
                context.Organizations.Add(organization2);
                context.Users.Add(new HighFiveUser() { Email = "a@b.com", Organization = organization });
                context.SaveChanges();
            
                var repo = new HighFiveRepository(context, _repoLogger);
                var controller = new UsersController(repo, _controllerLogger);

                var updatedUser = new UserViewModel() { Email = "a@b.com", OrganizationName = "IDontExist" };
                var result = controller.Put("a@b.com", updatedUser);
                result.Should().BeOfType<NotFoundObjectResult>();
                var notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("Organization IDontExist not found", notFoundResult.Value);
            }
        }

        [TestMethod]
        public void UsersController_Put_SimulateServerFailure()
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
                result.Should().BeOfType<NotFoundObjectResult>();
                var notFoundResult = result as NotFoundObjectResult;
                AssertMessageProperty("Organization IDontExist not found", notFoundResult.Value);
            }
        }

        #endregion

        #region utilities

        private void AssertMessageProperty(string expectedMessage, object result)
        {
            object actualMessage = result.GetType().GetProperty("Message").GetValue(result, null);
            actualMessage.Should().Be(expectedMessage as string);
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

        #endregion

    }
}
