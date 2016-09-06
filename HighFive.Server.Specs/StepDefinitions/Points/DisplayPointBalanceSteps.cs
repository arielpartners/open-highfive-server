
#region references

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.Controllers;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

#endregion

namespace HighFive.Server.Specs.StepDefinitions.Points
{
    [Binding]
    public class DisplayPointBalanceSteps
    {
        private HighFiveUser _highFiveUser;
        private UserViewModel _userViewModelOutput;
        private readonly DbContextOptions<HighFiveContext> _options;
        private Recognition _recognition;

        #region Constructor

        public DisplayPointBalanceSteps()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserViewModel, HighFiveUser>().ReverseMap();
            });

            _options = CreateNewContextOptions();
            _highFiveUser = new HighFiveUser();
        }

        #endregion

        [Given]
        public void Given_I_am_logged_in_as_the_following_user2(Table table)
        {
            var userViewModel = table.CreateInstance<UserViewModel>();

            // add recognition to the in memory repository
            using (var context = new HighFiveContext(_config, _options))
            {
                // Add user
                HighFiveUser user = new HighFiveUser { FirstName = "Clark", LastName = "Kent", Email = "clark@kent.com" };
                context.Users.Add(user);
                context.SaveChanges();
                user = context.Users.FirstOrDefault();

                // Add recoginition
                Recognition recoginition = new Recognition();
                recoginition.Receiver = user;
                recoginition.Points = 200;
                context.Recognitions.Add(recoginition);

                context.SaveChangesAsync();
            }
        }

        [When]
        public void When_I_view_my_point_balance()
        {
            using (var context = new HighFiveContext(_config, _options))
            {
                var repo = new HighFiveRepository(context, _repoLogger);
                var controller = new UsersController(repo, _controllerLogger);

                var result = controller.GetByEmail("clark@kent.com");
                result.Should().BeOfType<OkObjectResult>();
                var okResult = result as OkObjectResult;
                _userViewModelOutput = okResult.Value as UserViewModel;

                IEnumerable<Recognition> recognitionList = new List<Recognition>();
                recognitionList = repo.GetAllRecognitions();

                _recognition = null;

                foreach (Recognition rec in recognitionList)
                {
                    if (rec.Receiver.Email == "clark@kent.com")
                    {
                        _recognition = rec;
                    }
                }

                _userViewModelOutput.PointBalance = _recognition.Points;
            }
        }

        [Then]
        public void Then_I_should_see_the_following_point_balance(Table table)
        {
            _recognition.Should().NotBeNull();
            _recognition.Points.ShouldBeEquivalentTo(table.Rows[0]["Points"]);
        }

        #region CreateNewContextOptions

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

        private IConfigurationRoot _config => new Mock<IConfigurationRoot>().Object;
        private ILogger<HighFiveRepository> _repoLogger => new Mock<ILogger<HighFiveRepository>>().Object;
        private ILogger<UsersController> _controllerLogger => new Mock<ILogger<UsersController>>().Object;
    }
}
