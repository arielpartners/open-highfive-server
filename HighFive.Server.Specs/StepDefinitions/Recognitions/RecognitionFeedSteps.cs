using TechTalk.SpecFlow;

namespace HighFive.Server.Specs.StepDefinitions.Recognitions
{
    [Binding]
    public class RecognitionFeedSteps
    {
        [Given]
        public void Given_the_following_recognitions_exist_in_the_system(Table table)
        {
            ScenarioContext.Current.Pending();
            // for each item in the table, create a recognition
            //var recognitions = table.CreateSet<RecognitionViewModel>();
            // add recognitions to the in memory repository
        }
        
        [Then]
        public void Then_I_should_see_a_list_of_recognitions_sorted_most_recent_first(Table table)
        {
            ScenarioContext.Current.Pending();
            // call the recognitions controller getAll method
            // assert that you get the recognitions back in the correct order, reverse sorted by date
        }
    }
}
