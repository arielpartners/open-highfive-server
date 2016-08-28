﻿using System;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HighFive.Server.Services.Models;

namespace HighFive.Server.Specs.StepDefinitions
{
    [Binding]
    public class RecognitionFeedSteps
    {
        [Given]
        public void Given_I_am_logged_in_as_the_following_user(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [When]
        public void When_I_view_the_home_page()
        {
            ScenarioContext.Current.Pending();
        }

        [Then]
        public void Then_I_should_see_a_list_of_recognitions_sorted_by_date()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
