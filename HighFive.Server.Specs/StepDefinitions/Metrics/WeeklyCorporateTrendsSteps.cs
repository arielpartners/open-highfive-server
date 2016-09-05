using AutoMapper;
using FluentAssertions;
using HighFive.Server.Services.Models;
using HighFive.Server.Services.Utils;
using HighFive.Server.Web.Controllers;
using HighFive.Server.Web.Utils;
using HighFive.Server.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace HighFive.Server.Specs.StepDefinitions
{
    [Binding]
    public class WeeklyCorporateTrendsSteps
    {
        Mock<IHighFiveRepository> _repo = new Mock<IHighFiveRepository>();
        List<GroupedMetric> _metrics = new List<GroupedMetric>();
        IActionResult _result;


        [Given]
        public void Given_There_are_0_recognitions_in_the_database(Table table)
        {
            _metrics = table.CreateSet<GroupedMetric>() as List<GroupedMetric>;
            
            _repo.Setup(o => o.GetOrganizationByName(It.IsAny<string>())).Returns(new Organization());
            _repo.Setup(r => r.GetMetrics(It.IsAny<string>(), It.IsAny<int>())).Returns(_metrics);

        }

        [Given]
        public void Given_The_following_data_in_the_database(Table table)
        {
            Given_There_are_0_recognitions_in_the_database(table);
        }

        [When]
        public void When_I_look_at_the_weekly_ring()
        {
            var controller = new MetricsController(_repo.Object, _controllerLogger);
            _result = controller.GetMetricsByOrganization("myOrganization");
        }

        [Then]
        public void Then_I_will_see_P0_recognitions(int p0)
        {
            var result = _result as OkObjectResult;
            var model = result.Value as MetricsViewModel;
            model.Week.Count.Should().Equals(0);
        }

        [Then]
        public void Then_I_will_see_donut_slices_representing_P0_total_recognitions_with_the_following_data(int p0, Table table)
        {
            IEnumerable<GroupedMetric> metricsData = table.CreateSet<GroupedMetric>();

            var result = _result as OkObjectResult;
            var model = result.Value as MetricsViewModel;
            var week = model.Week;
            week.ShouldBeEquivalentTo<IEnumerable<GroupedMetric>>(metricsData);
            
        }

        #region properties

        private ILogger<MetricsController> _controllerLogger => new Mock<ILogger<MetricsController>>().Object;

        private ILogger<HighFiveRepository> _repoLogger => new Mock<ILogger<HighFiveRepository>>().Object;

        #endregion

        #region utils
        #endregion

    }


}
