using System;
using TechTalk.SpecFlow;

namespace HighFive.Server.Specs.StepDefinitions.Authentication
{
    [Binding]
    public class LoginSteps
    {
        [Given]
        public void Given_The_following_user_exists(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given]
        public void Given_I_fill_in_the_login_form_info(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given]
        public void Given_I_fill_in_the_login_information(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When]
        public void When_I_login()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_the_login_will_be_successful()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_the_following_information_will_be_returned(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_the_login_will_be_unsuccessful()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
