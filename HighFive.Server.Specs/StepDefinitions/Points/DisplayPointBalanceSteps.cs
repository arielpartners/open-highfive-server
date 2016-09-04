using System;
using TechTalk.SpecFlow;

namespace HighFive.Server.Specs.StepDefinitions.Points
{
    [Binding]
    public class DisplayPointBalanceSteps
    {
        [When]
        public void When_I_view_my_point_balance()
        {
            ScenarioContext.Current.Pending();
        }

        [Then]
        public void Then_I_should_see_the_following_point_balance(Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
