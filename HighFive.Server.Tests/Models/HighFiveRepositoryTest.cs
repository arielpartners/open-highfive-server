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
using HighFive.Server.Web.ViewModels;
using HighFive.Server.Web.Controllers;

#endregion

namespace HighFive.Server.Models
{
    [TestClass]
    public class HighFiveRepositoryTest
    {
        #region User Tests

        #region TestGetAllUsers()

        [TestMethod]
        public void TestGetAllUsers()
        {
            var options = CreateNewContextOptions();

            // user count should be zero to start
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                IEnumerable<HighFiveUser> userList = repo.GetAllUsers();
                List<HighFiveUser> userList2 = (List<HighFiveUser>)userList;
                Assert.AreEqual(0, userList2.Count);
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

                Assert.AreEqual(1, userList2.Count);

                // make sure the email address is the same as added. TODO, make this a lambda expression
                int userCount = 0;
                foreach (HighFiveUser highFiveUser in userList2)
                {
                    if(highFiveUser.Email== "a@b.com")
                    {
                        userCount++;
                    }
                }

                Assert.AreEqual(1, userCount);
            }
        }

        #endregion

        #region TestAddUser()

        [TestMethod]
        public void TestAddUser()
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

            highFiveUser = null;

            // get the user by email
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                Assert.IsNotNull(highFiveUser);
                Assert.AreEqual("clark.kent@metropolis.com", highFiveUser.Email);
                Assert.AreEqual("Ariel Partners", highFiveUser.Organization.Name);
            }
        }

        #endregion

        #region TestDeleteUser()

        [TestMethod]
        public void TestDeleteUser()
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

        #endregion

        #region TestForDuplicateUserEmails()

        [TestMethod]
        public void TestForDuplicateUserEmails()
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
                    .ShouldThrow<Exception>()
                    .WithMessage("Email for this user already exists in the database");
            }
        }

        #endregion

        #region TestUpdateUser()

        [TestMethod]
        public void TestUpdateUser()
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

        #endregion

        #region TestGetUserByEmail()

        [TestMethod]
        public void TestGetUserByEmail()
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

        #endregion

        #endregion

        #region Organization Tests

        #region TestGetOrganizationByName()

        [TestMethod]
        public void TestGetOrganizationByName()
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

        #endregion
        
        #region TestAddOrganization()

        [TestMethod]
        public void TestAddOrganization()
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

        #endregion

        #endregion

        #region CreateNewContextOptions()

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

        #region Mock properties

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

