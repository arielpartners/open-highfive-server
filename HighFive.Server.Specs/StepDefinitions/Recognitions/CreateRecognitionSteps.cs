using System;
using TechTalk.SpecFlow;

namespace HighFive.Server.Specs.StepDefinitions.Recognitions
{
    [Binding]
    public class CreateRecognitionSteps
    {
        [Given]
        public void Given_I_am_logged_in_as_the_following_user(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [When]
        public void When_I_create_the_following_recognition(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_the_system_should_confirm_that_the_following_recognition_has_been_created(Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
