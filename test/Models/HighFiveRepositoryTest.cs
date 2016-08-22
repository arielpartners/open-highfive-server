using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Collections.Generic;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using highfive_server.Controllers;

namespace highfive_server.Models
{
    public class Tests
    {
        #region TestGetAllUsers()

        [Fact]
        public void TestGetAllUsers()
        {
            var options = CreateNewContextOptions();

            // user count should be zero to start
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                IEnumerable<HighFiveUser> userList = repo.GetAllUsers();
                List<HighFiveUser> userList2 = (List<HighFiveUser>)userList;
                Assert.Equal(0, userList2.Count);
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

                Assert.Equal(1, userList2.Count);

                // make sure the email address is the same as added
                int userCount = 0;
                foreach (HighFiveUser highFiveUser in userList2)
                {
                    if(highFiveUser.Email== "a@b.com")
                    {
                        userCount++;
                    }
                }

                Assert.Equal(1, userCount);
            }
        }

        #endregion

        #region TestAddUser()

        [Fact]
        public void TestAddUser()
        {
            var options = CreateNewContextOptions();

            HighFiveUser highFiveUser;

            // make sure this user is not on file
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                Assert.Null(highFiveUser);
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
                Assert.NotNull(highFiveUser);
                Assert.Equal("clark.kent@metropolis.com", highFiveUser.Email);
                Assert.Equal("Ariel Partners", highFiveUser.Organization.Name);
            }
        }

        #endregion

        #region TestDeleteUser()

        [Fact]
        public void TestDeleteUser()
        {
            var options = CreateNewContextOptions();

            HighFiveUser highFiveUser;

            // make sure this user is not on file
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                Assert.Null(highFiveUser);
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
                Assert.NotNull(highFiveUser);
            }

            // make sure the user was deleted
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                repo.DeleteUser(highFiveUser);
                context.SaveChanges();
                highFiveUser = null;

                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                Assert.Null(highFiveUser);
            }
        }

        #endregion

        #region TestForDuplicateUserEmails()

        [Fact]
        public void TestForDuplicateUserEmails()
        {
            var options = CreateNewContextOptions();

            HighFiveUser highFiveUser;

            // make sure this user is not on file
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = repo.GetUserByEmail("clark.kent@metropolis.com");
                Assert.Null(highFiveUser);
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
                Assert.Throws<Exception>(() => { repo.AddUser(highFiveUser); });
            }
        }

        #endregion

        #region TestUpdateUser()

        [Fact]
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

            // read the user and make sure the email was updated
            using (var context = new HighFiveContext(_config, options))
            {
                HighFiveRepository repo = new HighFiveRepository(context, _repoLogger);
                highFiveUser = null;
                highFiveUser = repo.GetUserByEmail("clark.kent2@metropolis.com");
                Assert.Equal("clark.kent2@metropolis.com", highFiveUser.Email);
            }
        }

        #endregion

        #region TestGetUserByEmail()

        [Fact]
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
                Assert.NotNull(highFiveUser);
            }
        }

        #endregion

        #region TestGetOrganizationByName()

        [Fact]
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
                Assert.NotNull(organization);
            }
        }

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

        #region private Properties

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

