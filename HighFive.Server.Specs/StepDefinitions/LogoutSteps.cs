using System;
using TechTalk.SpecFlow;

namespace HighFive.Server.Specs.StepDefinitions
{
    [Binding]
    public class LogoutSteps
    {
        [Given]
        public void Given_the_following_logged_in_user(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When]
        public void When_I_logout()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_I_should_be_logged_out_of_the_system()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
