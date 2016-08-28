using System;
using TechTalk.SpecFlow;

namespace HighFive.Server.Specs.StepDefinitions.Administration
{
    [Binding]
    public class AdministerOrganizationSteps
    {
        [Given]
        public void Given_I_am_not_logged_in()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given]
        public void Given_I_attempt_to_create_a_new_organization(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_I_should_receive_an_error(Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
