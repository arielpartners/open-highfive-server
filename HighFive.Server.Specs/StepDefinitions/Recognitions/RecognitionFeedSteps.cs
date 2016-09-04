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
        private IEnumerable<RecognitionViewModel> _recognitions;
        private readonly DbContextOptions<HighFiveContext> _options;
        private IList<RecognitionViewModel> _recognitionOutputList;
        #region Constructor

        public RecognitionFeedSteps()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<RecognitionViewModel, Recognition>().ReverseMap();
                config.CreateMap<Recognition, RecognitionViewModel>()
                    .ForMember(g => g.SenderName, opt => opt.MapFrom(u => u.Sender.UserName))
                    .ForMember(g => g.ReceiverName, opt => opt.MapFrom(u => u.Receiver.UserName))
                    .ForMember(g => g.OrganizationName, opt => opt.MapFrom(u => u.Organization.Name))
                    .ForMember(g => g.CorporateValueName, opt => opt.MapFrom(u => u.Value.Name));
            });
            _options = CreateNewContextOptions();
        }

        #endregion

        [Given]
        public void Given_the_following_recognitions_exist_in_the_system(Table table)
        {
            _recognitions = table.CreateSet<RecognitionViewModel>();

            // add recognitions to the in memory repository
            using (var context = new HighFiveContext(_config, _options))
            {
                // for each item in the table, create a recognition
                foreach (object recViewModel in _recognitions)
                {
                    context.Recognitions.Add(Mapper.Map<Recognition>(recViewModel));
                }
                context.SaveChanges();
            }
        }

        [When]
        public void When_I_view_the_home_page()
        {
            using (var context = new HighFiveContext(_config, _options))
            {
                //var repo = new HighFiveRepository(context, _repoLogger);
                var repo = new Mock<IHighFiveRepository>();
                //repo.Setup(r => r.GetAllRecognitions()).Returns(_recognitions);
                var controller = new RecognitionsController(repo.Object, _controllerLogger);

                var result = controller.GetAll();
                result.Should().BeOfType<OkObjectResult>();
                var okResult = result as OkObjectResult;
                var lst = okResult.Value as IEnumerable<RecognitionViewModel>;
                _recognitionOutputList = lst.OrderByDescending(x => x.DateCreated).ToList();
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
                AssertMessageProperty(recognitionInputList[index].SenderName, recognitionOutputItem, "SenderName");
                AssertMessageProperty(recognitionInputList[index].ReceiverName, recognitionOutputItem, "ReceiverName");
                AssertMessageProperty(recognitionInputList[index].OrganizationName, recognitionOutputItem, "OrganizationName");
                AssertMessageProperty(recognitionInputList[index].CorporateValueName, recognitionOutputItem, "CorporateValueName");
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
