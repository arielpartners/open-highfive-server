using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using HighFive.Server.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using HighFive.Server.Web.Controllers;

namespace HighFive.Server.Specs.StepDefinitions.Recognitions
{
    [Binding]
    public class CreateRecognitionSteps
    {
        private RecognitionsController _controller;
        private IHighFiveRepository _repo;
        private IList<Recognition> _recognitions;
        //private HighFiveUser _loggedInUser;
        private readonly DbContextOptions<HighFiveContext> _options;
        private IList<RecognitionViewModel> _recognitionOutputList;
        private HighFiveContext _context;

        #region Constructor

        public CreateRecognitionSteps()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Recognition, RecognitionViewModel>()
                    .ForMember(g => g.SenderName, opt => opt.MapFrom(u => u.Sender.FirstName + " " + u.Sender.LastName))
                    .ForMember(g => g.SenderEmail, opt => opt.MapFrom(u => u.Sender.Email))
                    .ForMember(g => g.ReceiverName, opt => opt.MapFrom(u => u.Receiver.FirstName + " " + u.Receiver.LastName))
                    .ForMember(g => g.ReceiverEmail, opt => opt.MapFrom(u => u.Receiver.Email))
                    .ForMember(g => g.OrganizationName, opt => opt.MapFrom(u => u.Organization.Name))
                    .ForMember(g => g.CorporateValueName, opt => opt.MapFrom(u => u.Value.Name)).ReverseMap();
                config.CreateMap<UserViewModel, HighFiveUser>().ReverseMap();
            });

            _options = CreateNewContextOptions();
            _recognitions = new List<Recognition>();
            _context = new HighFiveContext(_config, _options);
            _repo = new HighFiveRepository(_context, _repoLogger);
            _controller = new RecognitionsController(_repo, _recognitionsControllerLogger);
        }

        #endregion

        [Given]
        public void Given_I_am_logged_in_as_the_following_user(Table table)
        {
            var user = table.CreateSet<UserViewModel>();
            // add recognition to the in memory repository
            
            //Add Organization
            var org = new Organization
            {
                Name = "Ariel Partners",
                Values = new List<CorporateValue>
                        {
                            new CorporateValue { Name="Commitment", Description="Committed to the long term success and happiness of our customers, our people, and our partners" },
                            new CorporateValue { Name="Courage", Description="To take on difficult challenges, to accept new ideas, to accept incremental failure" },
                            new CorporateValue { Name="Excellence", Description="Always strive to exceed expectations and continuously improve" },
                            new CorporateValue { Name="Integrity", Description="Always act honestly, ethically, and do the right thing even when it hurts" },
                            new CorporateValue { Name="Honesty", Description="Always act honestly, ethically, and do the right thing even when it hurts" },
                            new CorporateValue { Name="Vigilance", Description="Always act honestly, ethically, and do the right thing even when it hurts" },
                            new CorporateValue { Name="Respect", Description="Always act honestly, ethically, and do the right thing even when it hurts" }
                        }
            };
            _context.Organizations.Add(org);
            _context.SaveChanges();
            org = _context.Organizations.FirstOrDefault();
            //Add Users
            var testUsersLst = new List<HighFiveUser>
            {
                new HighFiveUser{UserName = "joe.blow@email.com",Email = "joe.blow@email.com",FirstName = "joe",LastName = "blow",Organization = org},
                new HighFiveUser{UserName = "suresh.nikam@email.com",Email = "suresh.nikam@email.com",FirstName = "suresh",LastName = "nikam",Organization = org}
            };
            foreach (var usr in testUsersLst)
            {
                _context.Users.Add(usr);
            }
            _context.SaveChanges();
            //Get the Logged in User who will become the sender
            //var repo = new HighFiveRepository(context, _repoLogger);
            //_sender = repo.GetUserByEmail(_loggedInUser.Email);
        }

        [When]
        public void When_I_create_the_following_recognition(Table table)
        {
            var recognitionsLst = table.CreateSet<RecognitionViewModel>();
 
            //Create the Recognition
            foreach (RecognitionViewModel recViewModel in recognitionsLst)
            {
                _controller.Post(recViewModel);
            }   
        }

        [Then]
        public void Then_the_system_should_confirm_that_the_following_recognition_has_been_created(Table table)
        {
            var theRecognition = table.CreateInstance<RecognitionViewModel>();
            
            //Get the Recognition
            var result = _controller.GetAll();
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            //var lst = okResult.Value as IEnumerable<RecognitionViewModel>;
            //_recognitionOutputList = lst.OrderByDescending(x => x.DateCreated).ToList();
            _recognitionOutputList = okResult.Value as IList<RecognitionViewModel>;

            _recognitionOutputList.Should().Contain(rec =>
                rec.ReceiverEmail.Equals(theRecognition.ReceiverEmail)
                && rec.SenderEmail.Equals(theRecognition.SenderEmail)
                && rec.CorporateValueName.Equals(theRecognition.CorporateValueName)
                );


        }

        [Given]
        public void Given_the_following_user_does_not_exist(Table table)
        {
            var nonExistingUser = new HighFiveUser { Email = "nobody@nowhere.com" };

            HighFiveRepository repo = new HighFiveRepository(_context, _repoLogger);
            UsersController controller = new UsersController(repo, _usersControllerLogger);

            var result = controller.GetByEmail(nonExistingUser.Email);
                
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            AssertMessageProperty("User " + nonExistingUser.Email + " not found", notFoundResult.Value);
        }

        [Then]
        public void Then_the_following_user_should_exist(Table table)
        {
            HighFiveUser user = table.CreateInstance<HighFiveUser>();
            
            HighFiveRepository repo = new HighFiveRepository(_context, _repoLogger);
            UsersController controller = new UsersController(repo, _usersControllerLogger);

            var result = controller.GetByEmail(user.Email);
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var userViewModel = okResult.Value as UserViewModel;
            userViewModel.Email.Should().Be(user.Email);
        }


        #region utilities

        private void AssertMessageProperty(string expectedMessage, object result, string propertyName = "Message")
        {
            var actualMessage = result.GetType().GetProperty(propertyName).GetValue(result, null);
            actualMessage.Should().Be(expectedMessage);
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

        private IConfigurationRoot _config => new Mock<IConfigurationRoot>().Object;

        private ILogger<RecognitionsController> _recognitionsControllerLogger => new Mock<ILogger<RecognitionsController>>().Object;
        private ILogger<UsersController> _usersControllerLogger => new Mock<ILogger<UsersController>>().Object;

        private ILogger<HighFiveRepository> _repoLogger => new Mock<ILogger<HighFiveRepository>>().Object;

        #endregion
    }
}
