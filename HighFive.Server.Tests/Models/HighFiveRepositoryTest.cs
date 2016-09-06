#region references

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.Controllers;
using HighFive.Server.Services.Utils;
using System.Linq;

#endregion

namespace HighFive.Server.Models
{
    [TestClass]
    public class HighFiveRepositoryTest
    {
        #region Repository_s

        [TestMethod]
        public void Repository_GetAllUsers()
        {
            var options = CreateNewContextOptions();

            // user count should be zero to start
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                IEnumerable<HighFiveUser> userList = repo.GetAllUsers();
                List<HighFiveUser> userList2 = (List<HighFiveUser>)userList;
                userList2.Should().BeEmpty();
            }

            // add a user
            using (var context = new HighFiveContext(_config, options))
            {
                context.Users.Add(new HighFiveUser { Email = "a@b.com" });
                context.SaveChanges();
            }

            // test only one user was added and the email address is the same as added
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                IEnumerable<HighFiveUser> userList = repo.GetAllUsers();
                List<HighFiveUser> userList2 = (List<HighFiveUser>)userList;

                //Assert.AreEqual(1, userList2.Count);
                userList2.Should().HaveCount(1);

                // make sure the email address is the same as added. TODO, make this a lambda expression
                int userCount = 0;
                foreach (HighFiveUser highFiveUser in userList2)
                {
                    if (highFiveUser.Email == "a@b.com")
                    {
                        userCount++;
                    }
                }

                Assert.AreEqual(1, userCount);
            }
        }

        [TestMethod]
        public void Repository_AddUser()
        {
            var options = CreateNewContextOptions();

            HighFiveUser highFiveUser;

            // make sure this user is not on file
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                //Assert.IsNull(highFiveUser);
                highFiveUser.Should().BeNull();
            }

            // add the user
            highFiveUser = new HighFiveUser();
            highFiveUser.Organization = new Organization();
            highFiveUser.Email = "clark.kent@metropolis.com";
            highFiveUser.Organization.Name = "Ariel Partners";

            using (var context = new HighFiveContext(_config, options))
            {
                context.Users.Add(highFiveUser);
                context.SaveChanges();
            }

            highFiveUser = null;

            // get the user by email
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                Assert.IsNotNull(highFiveUser);
                highFiveUser.Email.Should().Be("clark.kent@metropolis.com");
                Assert.AreEqual("Ariel Partners", highFiveUser.Organization.Name);
            }
        }

        [TestMethod]
        public void Repository_DeleteUser()
        {
            var options = CreateNewContextOptions();

            HighFiveUser highFiveUser;

            // make sure this user is not on file
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                Assert.IsNull(highFiveUser);
            }

            // add the user
            highFiveUser = new HighFiveUser();
            highFiveUser.Organization = new Organization();
            highFiveUser.Email = "clark.kent@metropolis.com";
            highFiveUser.Organization.Name = "Ariel Partners";

            using (var context = new HighFiveContext(_config, options))
            {
                context.Users.Add(highFiveUser);
                context.SaveChanges();
            }

            // confirm users was added
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                Assert.IsNotNull(highFiveUser);
            }

            // make sure the user was deleted
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                repo.DeleteUser(highFiveUser);
                context.SaveChanges();
                highFiveUser = null;

                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                Assert.IsNull(highFiveUser);
            }
        }

        [TestMethod]
        public void Repository_ForDuplicateUserEmails()
        {
            var options = CreateNewContextOptions();

            HighFiveUser highFiveUser;

            // make sure this user is not on file
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                Assert.IsNull(highFiveUser);
            }

            // add the user
            highFiveUser = new HighFiveUser();
            highFiveUser.Organization = new Organization();
            highFiveUser.Email = "clark.kent@metropolis.com";
            highFiveUser.Organization.Name = "Ariel Partners";

            using (var context = new HighFiveContext(_config, options))
            {
                context.Users.Add(highFiveUser);
                context.SaveChanges();
            }

            // check the repo add user method won't add a duplicate email
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                //Assert.Throws<Exception>(() => { repo.AddUser(highFiveUser); });

                repo.Invoking(y => y.AddUser(highFiveUser))
                    .ShouldThrow<HighFiveException>()
                    .WithMessage("User clark.kent@metropolis.com already exists in the database");
            }
        }

        [TestMethod]
        public void Repository_UpdateUser()
        {
            var options = CreateNewContextOptions();

            // add the user
            HighFiveUser highFiveUser = new HighFiveUser();
            highFiveUser.Organization = new Organization();
            highFiveUser.Email = "clark.kent@metropolis.com";
            highFiveUser.Organization.Name = "Ariel Partners";

            using (var context = new HighFiveContext(_config, options))
            {
                context.Users.Add(highFiveUser);
                context.SaveChanges();
            }

            highFiveUser = null;

            // update the user's email address
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                highFiveUser.Email = "clark.kent2@metropolis.com";
                context.Update(highFiveUser);
                context.SaveChanges();
            }

            //TODO - add a test too make sure original email clark.kent@metropolis.com is not on file

            // read the user and make sure the email was updated
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = null;
                highFiveUser = repo.GetUserByEmail("clark.kent2@metropolis.com");
                Assert.AreEqual("clark.kent2@metropolis.com", highFiveUser.Email);
            }
        }

        [TestMethod]
        public void Repository_GetUserByEmail()
        {
            var options = CreateNewContextOptions();

            // add a user
            HighFiveUser highFiveUser = new HighFiveUser();
            highFiveUser.Organization = new Organization();
            highFiveUser.Email = "clark.kent@metropolis.com";
            highFiveUser.Organization.Name = "Ariel Partners";

            using (var context = new HighFiveContext(_config, options))
            {
                context.Users.Add(highFiveUser);
                context.SaveChanges();
            }

            highFiveUser = null;

            // make sure the user was added
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                Assert.IsNotNull(highFiveUser);
            }
        }

        [TestMethod]
        public void Repository_GetOrganizationByName()
        {
            var options = CreateNewContextOptions();

            // add the organization
            Organization organization = new Organization();
            organization.Name = "Ariel Partners";

            using (var context = new HighFiveContext(_config, options))
            {
                context.Organizations.Add(organization);
                context.SaveChanges();
            }

            organization = null;

            // make sure the organization was added
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                organization = repo.GetOrganizationByName("Ariel Partners");
                Assert.IsNotNull(organization);
            }
        }

        [TestMethod]
        public void Repository_AddOrganization()
        {
            var options = CreateNewContextOptions();

            Organization organization;

            // make sure this user is not on file
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                organization = repo.GetOrganizationByName("Macys");
                Assert.IsNull(organization);
            }

            // add the organization
            organization = new Organization();
            organization.Name = "Macys";
            Guid organizationId;

            using (var context = new HighFiveContext(_config, options))
            {
                var abc = context.Organizations.Add(organization);
                context.SaveChanges();
                organizationId = organization.Id;
            }

            organization = null;

            // get the user by email
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                organization = repo.GetOrganizationByName("Macys");
                Assert.IsNotNull(organization);
                Assert.AreEqual("Macys", organization.Name);
                Assert.AreEqual(organizationId, organization.Id);
            }
        }

        [TestMethod]
        public void Repository_IsConnected()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new HighFiveRepository(context, _repoLogger);
                repo.IsConnected();
            }
        }

        [TestMethod]
        public void Repository_AddCorporateValue()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new HighFiveRepository(context, _repoLogger);
                repo.AddCorporateValue(new CorporateValue {Name = "Corporate Value1"});
            }
        }

        [TestMethod]
        public void Repository_GetWeekMetrics()
        {
            var options = CreateNewContextOptions();
            using (var context = new HighFiveContext(_config, options))
            {
                var repo = new HighFiveRepository(context, _repoLogger);

                var corpVal1 = new CorporateValue { Name = "Corporate Value1" };
                var corpVal2 = new CorporateValue { Name = "Corporate Value2" };
                var corpVal3 = new CorporateValue { Name = "Corporate Value3" };

                ICollection<CorporateValue> corpVals = new List<CorporateValue>();
                corpVals.Add(corpVal1);
                corpVals.Add(corpVal2);
                corpVals.Add(corpVal3);

                repo.AddCorporateValue(corpVal1);
                repo.AddCorporateValue(corpVal2);
                repo.AddCorporateValue(corpVal3);
                repo.SaveChanges();

                Organization org = new Organization { Name = "TestOrg", Values = corpVals };

                repo.AddOrganization(org);

                //DateCreated will be within a week since it is set to now
                repo.AddRecognition(new Recognition { Value = repo.GetCorporateValueByName("Corporate Value1"), Organization = org, Points = 1 });
                repo.AddRecognition(new Recognition { Value = repo.GetCorporateValueByName("Corporate Value1"), Organization = org, Points = 1 });
                repo.AddRecognition(new Recognition { Value = repo.GetCorporateValueByName("Corporate Value2"), Organization = org, Points = 3 });
                repo.AddRecognition(new Recognition { Value = repo.GetCorporateValueByName("Corporate Value3"), Organization = org, Points = 5 });

                repo.SaveChanges();
                var weekMetrics = repo.GetMetrics(org.Name, 7);
                weekMetrics.ToList().Count.Should().Equals(3);
                var met1 = weekMetrics.FirstOrDefault(m => m.CorporateValue == "Corporate Value1");
                met1.Points.Should().Equals(2);
                var met2 = weekMetrics.FirstOrDefault(m => m.CorporateValue == "Corporate Value2");
                met1.Points.Should().Equals(2);
                var met3 = weekMetrics.FirstOrDefault(m => m.CorporateValue == "Corporate Value3");
                met1.Points.Should().Equals(2);
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

        private IConfigurationRoot _config => new Mock<IConfigurationRoot>().Object;

        private ILogger<UsersController> _controllerLogger => new Mock<ILogger<UsersController>>().Object;

        private ILogger<HighFiveRepository> _repoLogger => new Mock<ILogger<HighFiveRepository>>().Object;

        #endregion
    }
}

