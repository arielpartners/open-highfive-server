﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace HighFive.Server.Specs.Features.Recognitions
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [TechTalk.SpecRun.FeatureAttribute("RecognitionFeed", Description="\tIn order to stay informed of the kudos that are being sent to people in my organ" +
        "ization\r\n\tAs a user\r\n\tI want to be see the most recent recognitions", SourceFile="Features\\Recognitions\\RecognitionFeed.feature", SourceLine=0)]
    public partial class RecognitionFeedFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "RecognitionFeed.feature"
#line hidden
        
        [TechTalk.SpecRun.FeatureInitialize()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "RecognitionFeed", "\tIn order to stay informed of the kudos that are being sent to people in my organ" +
                    "ization\r\n\tAs a user\r\n\tI want to be see the most recent recognitions", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [TechTalk.SpecRun.FeatureCleanup()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        [TechTalk.SpecRun.ScenarioCleanup()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "SenderName",
                        "ReceiverName",
                        "OrganizationName",
                        "CorporateValueName",
                        "Points",
                        "DateCreated",
                        "Description"});
            table1.AddRow(new string[] {
                        "joe@email.com",
                        "suresh@email.com",
                        "Ariel Partners",
                        "Integrity",
                        "30",
                        "8/2/2016 08:15:00",
                        "Great job!"});
            table1.AddRow(new string[] {
                        "matthew@email.com",
                        "sue@email.com",
                        "Ariel Partners",
                        "Honesty",
                        "10",
                        "8/7/2016 14:21:00",
                        "Great job!"});
            table1.AddRow(new string[] {
                        "sue@email.com",
                        "dave@email.com",
                        "Ariel Partners",
                        "Vigilance",
                        "50",
                        "8/1/2016 10:15:00",
                        "fantastic!"});
            table1.AddRow(new string[] {
                        "nikhil@email.com",
                        "jose@email.com",
                        "Ariel Partners",
                        "Respect",
                        "15",
                        "8/2/2016 19:04:00",
                        "don\'t know what i would do without you"});
            table1.AddRow(new string[] {
                        "john@email.com",
                        "tom@email.com",
                        "Ariel Partners",
                        "Excellence",
                        "70",
                        "8/4/2016 09:44:00",
                        "ipsum laurem"});
#line 7
testRunner.Given("the following recognitions exist in the system:", ((string)(null)), table1, "Given ");
#line hidden
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("View All Recognitions", new string[] {
                "framework"}, SourceLine=15)]
        public virtual void ViewAllRecognitions()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("View All Recognitions", new string[] {
                        "framework"});
#line 16
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 17
 testRunner.When("I view the home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "SenderName",
                        "ReceiverName",
                        "OrganizationName",
                        "CorporateValueName",
                        "Points",
                        "DateCreated",
                        "Description"});
            table2.AddRow(new string[] {
                        "matthew@email.com",
                        "sue@email.com",
                        "Ariel Partners",
                        "Honesty",
                        "10",
                        "8/7/2016 14:21:00",
                        "Great job!"});
            table2.AddRow(new string[] {
                        "john@email.com",
                        "tom@email.com",
                        "Ariel Partners",
                        "Excellence",
                        "70",
                        "8/4/2016 09:44:00",
                        "ipsum laurem"});
            table2.AddRow(new string[] {
                        "nikhil@email.com",
                        "jose@email.com",
                        "Ariel Partners",
                        "Respect",
                        "15",
                        "8/2/2016 19:04:00",
                        "don\'t know what i would do without you"});
            table2.AddRow(new string[] {
                        "joe@email.com",
                        "suresh@email.com",
                        "Ariel Partners",
                        "Integrity",
                        "30",
                        "8/2/2016 08:15:00",
                        "Great job!"});
            table2.AddRow(new string[] {
                        "sue@email.com",
                        "dave@email.com",
                        "Ariel Partners",
                        "Vigilance",
                        "50",
                        "8/1/2016 10:15:00",
                        "fantastic!"});
#line 18
 testRunner.Then("I should see a list of recognitions sorted most recent first:", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.TestRunCleanup()]
        public virtual void TestRunCleanup()
        {
            TechTalk.SpecFlow.TestRunnerManager.GetTestRunner().OnTestRunEnd();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class RecognitionFeedFeature_MsTest
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "RecognitionFeed.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "RecognitionFeed", "\tIn order to stay informed of the kudos that are being sent to people in my organ" +
                    "ization\r\n\tAs a user\r\n\tI want to be see the most recent recognitions", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "RecognitionFeed")))
            {
                HighFive.Server.Specs.Features.Recognitions.RecognitionFeedFeature_MsTest.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "SenderName",
                        "ReceiverName",
                        "OrganizationName",
                        "CorporateValueName",
                        "Points",
                        "DateCreated",
                        "Description"});
            table1.AddRow(new string[] {
                        "joe@email.com",
                        "suresh@email.com",
                        "Ariel Partners",
                        "Integrity",
                        "30",
                        "8/2/2016 08:15:00",
                        "Great job!"});
            table1.AddRow(new string[] {
                        "matthew@email.com",
                        "sue@email.com",
                        "Ariel Partners",
                        "Honesty",
                        "10",
                        "8/7/2016 14:21:00",
                        "Great job!"});
            table1.AddRow(new string[] {
                        "sue@email.com",
                        "dave@email.com",
                        "Ariel Partners",
                        "Vigilance",
                        "50",
                        "8/1/2016 10:15:00",
                        "fantastic!"});
            table1.AddRow(new string[] {
                        "nikhil@email.com",
                        "jose@email.com",
                        "Ariel Partners",
                        "Respect",
                        "15",
                        "8/2/2016 19:04:00",
                        "don\'t know what i would do without you"});
            table1.AddRow(new string[] {
                        "john@email.com",
                        "tom@email.com",
                        "Ariel Partners",
                        "Excellence",
                        "70",
                        "8/4/2016 09:44:00",
                        "ipsum laurem"});
#line 7
testRunner.Given("the following recognitions exist in the system:", ((string)(null)), table1, "Given ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("View All Recognitions")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "RecognitionFeed")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("framework")]
        public virtual void ViewAllRecognitions()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("View All Recognitions", new string[] {
                        "framework"});
#line 16
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 17
 testRunner.When("I view the home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "SenderName",
                        "ReceiverName",
                        "OrganizationName",
                        "CorporateValueName",
                        "Points",
                        "DateCreated",
                        "Description"});
            table2.AddRow(new string[] {
                        "matthew@email.com",
                        "sue@email.com",
                        "Ariel Partners",
                        "Honesty",
                        "10",
                        "8/7/2016 14:21:00",
                        "Great job!"});
            table2.AddRow(new string[] {
                        "john@email.com",
                        "tom@email.com",
                        "Ariel Partners",
                        "Excellence",
                        "70",
                        "8/4/2016 09:44:00",
                        "ipsum laurem"});
            table2.AddRow(new string[] {
                        "nikhil@email.com",
                        "jose@email.com",
                        "Ariel Partners",
                        "Respect",
                        "15",
                        "8/2/2016 19:04:00",
                        "don\'t know what i would do without you"});
            table2.AddRow(new string[] {
                        "joe@email.com",
                        "suresh@email.com",
                        "Ariel Partners",
                        "Integrity",
                        "30",
                        "8/2/2016 08:15:00",
                        "Great job!"});
            table2.AddRow(new string[] {
                        "sue@email.com",
                        "dave@email.com",
                        "Ariel Partners",
                        "Vigilance",
                        "50",
                        "8/1/2016 10:15:00",
                        "fantastic!"});
#line 18
 testRunner.Then("I should see a list of recognitions sorted most recent first:", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
