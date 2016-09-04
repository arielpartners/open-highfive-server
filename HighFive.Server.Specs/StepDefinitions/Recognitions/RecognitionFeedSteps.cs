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

namespace HighFive.Server.Specs.StepDefinitions.Recognitions
{
    [Binding]
    public class RecognitionFeedSteps
    {
        private IList<Recognition> _recognitions;
        private readonly DbContextOptions<HighFiveContext> _options;
        private IList<RecognitionViewModel> _recognitionOutputList;

        #region Constructor

        public RecognitionFeedSteps()
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
        }

        #endregion

        [Given]
        public void Given_the_following_recognitions_exist_in_the_system(Table table)
        {
            var recognitionsLst = table.CreateSet<RecognitionViewModel>();

            // add recognitions to the in memory repository
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
                    new HighFiveUser{UserName = "joe@email.com",Email = "joe@email.com",FirstName = "joe",LastName = "",Organization = org},
                    new HighFiveUser{UserName = "suresh@email.com",Email = "suresh@email.com",FirstName = "suresh",LastName = "",Organization = org},
                    new HighFiveUser{UserName = "matthew@email.com",Email = "matthew@email.com",FirstName = "matthew",LastName = "",Organization = org},
                    new HighFiveUser{UserName = "sue@email.com",Email = "sue@email.com",FirstName = "sue",LastName = "",Organization = org},
                    new HighFiveUser{UserName = "dave@email.com",Email = "dave@email.com",FirstName = "dave",LastName = "",Organization = org},
                    new HighFiveUser{UserName = "nikhil@email.com",Email = "nikhil@email.com",FirstName = "nikhil",LastName = "",Organization = org},
                    new HighFiveUser{UserName = "jose@email.com",Email = "jose@email.com",FirstName = "jose",LastName = "",Organization = org},
                    new HighFiveUser{UserName = "tom@email.com",Email = "tom@email.com",FirstName = "tom",LastName = "",Organization = org},
                    new HighFiveUser{UserName = "john@email.com",Email = "john@email.com",FirstName = "john",LastName = "",Organization = org}
                };
                foreach (var usr in testUsersLst)
                {
                    context.Users.Add(usr);
                }
                context.SaveChanges();
                //Create the Recognitions
                var repo = new HighFiveRepository(context, _repoLogger);
                _recognitions = new List<Recognition>();
                // for each item in the table, create a recognition
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
            }
        }

        [When]
        public void When_I_view_the_home_page()
        {
            using (var context = new HighFiveContext(_config, _options))
            {
                var repo = new HighFiveRepository(context, _repoLogger);
                //var repo = new Mock<IHighFiveRepository>();
                //repo.Setup(r => r.GetAllRecognitions()).Returns(_recognitions);
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
        public void Then_I_should_see_a_list_of_recognitions_sorted_most_recent_first(Table table)
        {
            // call the recognitions controller getAll method
            // assert that you get the recognitions back in the correct order, reverse sorted by date
            var recognitionInputList = _recognitions.OrderByDescending(x => x.DateCreated).ToArray();
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

        private void AssertMessageProperty(string expectedMessage, object result, string propertyName= "Message")
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
