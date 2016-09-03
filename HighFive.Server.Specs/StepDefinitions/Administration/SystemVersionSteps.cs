using FluentAssertions;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using System.Collections.Generic;

namespace HighFive.Server.Specs.StepDefinitions.Administration
{
    [Binding]
    public class SystemVersionSteps
    {
        private string _version;
        private IConfigurationRoot _config;

        [Given]
        public void Given_The_system_has_version_P0(String P0)
        {
            var config = new Dictionary<string, string>()
            {
                ["version"] = P0
            };
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(config);
            _config = configurationBuilder.Build();
        }
        
        [When]
        public void When_I_view_the_version_number()
        {
            var repo = new Mock<IHighFiveRepository>();
            repo.Setup(r => r.IsConnected()).Returns(Task.FromResult(true));

            var controller = new HealthCheckController(_config, repo.Object, _logger);
            var result = controller.Ping() as Task<IActionResult>;
            var viewresult = result.Result;
            var okObjectResult = viewresult as OkObjectResult;
            _version = GetPropertyOfAnonymousObject(okObjectResult.Value, "Version") as string;
        }
        
        [Then]
        public void Then_the_version_should_be_P0(String P0)
        {
            _version.Should().Be(P0);
        }

        private object GetPropertyOfAnonymousObject(object o, string propertyName)
        {
            return o.GetType().GetProperty(propertyName).GetValue(o, null);
        }

        private ILogger<HealthCheckController> _logger => new Mock<ILogger<HealthCheckController>>().Object;
    }
}
