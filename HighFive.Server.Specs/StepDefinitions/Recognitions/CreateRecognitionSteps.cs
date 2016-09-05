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
        private IList<Recognition> _recognitions;
        //private HighFiveUser _loggedInUser;
        private readonly DbContextOptions<HighFiveContext> _options;
        private IList<RecognitionViewModel> _recognitionOutputList;

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
            });
            _options = CreateNewContextOptions();
            _recognitions = new List<Recognition>();
        }

        #endregion

        [Given]
        public void Given_I_am_logged_in_as_the_following_user(Table table)
        {
            var user = table.CreateSet<UserViewModel>();
            // add recognition to the in memory repository
            using (var context = new HighFiveContext(_config, _options))
            {
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
                context.Organizations.Add(org);
                context.SaveChanges();
                org = context.Organizations.FirstOrDefault();
                //Add Users
                var testUsersLst = new List<HighFiveUser>
                {
                    new HighFiveUser{UserName = "joe.blow@email.com",Email = "joe.blow@email.com",FirstName = "joe",LastName = "blow",Organization = org},
                    new HighFiveUser{UserName = "suresh.nikam@email.com",Email = "suresh.nikam@email.com",FirstName = "suresh",LastName = "nikam",Organization = org}
                };
                foreach (var usr in testUsersLst)
                {
                    context.Users.Add(usr);
                }
                context.SaveChanges();
                //Get the Logged in User who will become the sender
                //var repo = new HighFiveRepository(context, _repoLogger);
                //_sender = repo.GetUserByEmail(_loggedInUser.Email);
            }
        }

        [When]
        public void When_I_create_the_following_recognition(Table table)
        {
            var recognitionsLst = table.CreateSet<RecognitionViewModel>();

            // add recognition to the in memory repository
            using (var context = new HighFiveContext(_config, _options))
            {
                var repo = new HighFiveRepository(context, _repoLogger);
                //Create the Recognition
                foreach (RecognitionViewModel recViewModel in recognitionsLst)
                {
                    var obj = Mapper.Map<Recognition>(recViewModel);
                    obj.Sender = repo.GetUserByEmail(recViewModel.SenderEmail);
                    obj.Receiver = repo.GetUserByEmail(recViewModel.ReceiverEmail);
                    obj.Organization = repo.GetOrganizationByName(recViewModel.OrganizationName);
                    obj.Value = repo.GetCorporateValueByName(recViewModel.CorporateValueName);
                    _recognitions.Add(obj);
                }
                context.Recognitions.AddRange(_recognitions);
                context.SaveChangesAsync();

                //Get the Recognition
                var controller = new RecognitionsController(repo, _controllerLogger);

                var result = controller.GetAll();
                result.Should().BeOfType<OkObjectResult>();
                var okResult = result as OkObjectResult;
                //var lst = okResult.Value as IEnumerable<RecognitionViewModel>;
                //_recognitionOutputList = lst.OrderByDescending(x => x.DateCreated).ToList();
                _recognitionOutputList = okResult.Value as IList<RecognitionViewModel>;
            }
        }

        [Then]
        public void Then_the_system_should_confirm_that_the_following_recognition_has_been_created(Table table)
        {
            var recognitionInputList = _recognitions.ToArray();
            var index = 0;
            foreach (var recognitionOutputItem in _recognitionOutputList)
            {
                AssertMessageProperty(recognitionInputList[index].Sender.Email, recognitionOutputItem, "SenderEmail");
                AssertMessageProperty(recognitionInputList[index].Receiver.Email, recognitionOutputItem, "ReceiverEmail");
                AssertMessageProperty(recognitionInputList[index].Organization.Name, recognitionOutputItem, "OrganizationName");
                AssertMessageProperty(recognitionInputList[index].Value.Name, recognitionOutputItem, "CorporateValueName");
                //AssertMessageProperty(recognitionInputList[index].DateCreated.ToString(), recognitionOutputItem, "DateCreated");
                AssertMessageProperty(recognitionInputList[index].Description, recognitionOutputItem, "Description");
                index++;
            }
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

        private ILogger<RecognitionsController> _controllerLogger => new Mock<ILogger<RecognitionsController>>().Object;

        private ILogger<HighFiveRepository> _repoLogger => new Mock<ILogger<HighFiveRepository>>().Object;

        #endregion
    }
}
