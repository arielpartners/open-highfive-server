using System;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HighFive.Server.Models;

namespace HighFive.Server.Specs.StepDefinitions
{
    [Binding]
    public class RecognitionFeedSteps
    {
        private int result { get; set; }
        private Calculator calculator = new Calculator();

        [Given]
        public void Given_I_have_entered_P0_into_the_calculator(int p0)
        {
            calculator.FirstNumber = p0;
        }

        [Given]
        public void Given_I_have_also_entered_P0_into_the_calculator(int p0)
        {
            calculator.SecondNumber = p0;
        }


        [When]
        public void When_I_press_add()
        {
            result = calculator.Add();
        }
        
        [Then]
        public void Then_the_result_should_be_P0_on_the_screen(int p0)
        {
            Assert.AreEqual(p0, result);
        }
    }
}
