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
namespace HighFive.Server.Specs.Features.Setup
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [TechTalk.SpecRun.FeatureAttribute("Login", Description="\tIn order to see my kudos\r\n\tAs a user\r\n\tI want to login to the system", SourceFile="Features\\Setup\\SeedData.feature", SourceLine=0)]
    public partial class LoginFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SeedData.feature"
#line hidden
        
        [TechTalk.SpecRun.FeatureInitialize()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Login", "\tIn order to see my kudos\r\n\tAs a user\r\n\tI want to login to the system", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        "Email",
                        "Password",
                        "FirstName",
                        "LastName",
                        "Organization",
                        "Password"});
            table1.AddRow(new string[] {
                        "test.user@hq.dhs.gov",
                        "password",
                        "Test",
                        "User",
                        "dhs",
                        "password"});
#line 7
 testRunner.Given("The following user exists:", ((string)(null)), table1, "Given ");
#line hidden
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("Successful login", SourceLine=11)]
        [TechTalk.SpecRun.IgnoreAttribute()]
        public virtual void SuccessfulLogin()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Successful login", new string[] {
                        "ignore"});
#line 12
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Email",
                        "Password"});
            table2.AddRow(new string[] {
                        "test.user@email.com",
                        "password"});
#line 13
 testRunner.When("I login with the following information:", ((string)(null)), table2, "When ");
#line 17
 testRunner.Then("the login will be successful", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Email",
                        "Password",
                        "FirstName",
                        "LastName",
                        "Organization"});
            table3.AddRow(new string[] {
                        "test.user@email.com",
                        "password",
                        "Test",
                        "User",
                        "dhs"});
#line 18
 testRunner.And("the following information will be returned", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [TechTalk.SpecRun.ScenarioAttribute("View All my Recognitions", SourceLine=22)]
        [TechTalk.SpecRun.IgnoreAttribute()]
        public virtual void ViewAllMyRecognitions()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("View All my Recognitions", new string[] {
                        "ignore"});
#line 23
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 24
 testRunner.When("I view the home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sender Email",
                        "Receiver Email",
                        "Organization Name",
                        "CorporateValue Name",
                        "Points",
                        "DateCreated",
                        "Description"});
            table4.AddRow(new string[] {
                        "matthew@email.com",
                        "sue@email.com",
                        "dhs",
                        "Honesty",
                        "10",
                        "8/7/2016 14:21:00",
                        "Great job!"});
            table4.AddRow(new string[] {
                        "john@email.com",
                        "tom@email.com",
                        "dhs",
                        "Excellence",
                        "70",
                        "8/4/2016 09:44:00",
                        "ipsum laurem"});
            table4.AddRow(new string[] {
                        "nikhil@email.com",
                        "jose@email.com",
                        "dhs",
                        "Respect",
                        "15",
                        "8/2/2016 19:04:00",
                        "don\'t know what i would do without you"});
            table4.AddRow(new string[] {
                        "joe@email.com",
                        "suresh@email.com",
                        "dhs",
                        "Integrity",
                        "30",
                        "8/2/2016 08:15:00",
                        "Great job!"});
#line 25
 testRunner.Then("I should see a list of my recognitions sorted most recent first:", ((string)(null)), table4, "Then ");
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
    public partial class LoginFeature_MsTest
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SeedData.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Login", "\tIn order to see my kudos\r\n\tAs a user\r\n\tI want to login to the system", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Login")))
            {
                HighFive.Server.Specs.Features.Setup.LoginFeature_MsTest.FeatureSetup(null);
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
                        "Email",
                        "Password",
                        "FirstName",
                        "LastName",
                        "Organization",
                        "Password"});
            table1.AddRow(new string[] {
                        "test.user@hq.dhs.gov",
                        "password",
                        "Test",
                        "User",
                        "dhs",
                        "password"});
#line 7
 testRunner.Given("The following user exists:", ((string)(null)), table1, "Given ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Successful login")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Login")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute()]
        public virtual void SuccessfulLogin()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Successful login", new string[] {
                        "ignore"});
#line 12
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Email",
                        "Password"});
            table2.AddRow(new string[] {
                        "test.user@email.com",
                        "password"});
#line 13
 testRunner.When("I login with the following information:", ((string)(null)), table2, "When ");
#line 17
 testRunner.Then("the login will be successful", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Email",
                        "Password",
                        "FirstName",
                        "LastName",
                        "Organization"});
            table3.AddRow(new string[] {
                        "test.user@email.com",
                        "password",
                        "Test",
                        "User",
                        "dhs"});
#line 18
 testRunner.And("the following information will be returned", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("View All my Recognitions")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Login")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute()]
        public virtual void ViewAllMyRecognitions()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("View All my Recognitions", new string[] {
                        "ignore"});
#line 23
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 24
 testRunner.When("I view the home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Sender Email",
                        "Receiver Email",
                        "Organization Name",
                        "CorporateValue Name",
                        "Points",
                        "DateCreated",
                        "Description"});
            table4.AddRow(new string[] {
                        "matthew@email.com",
                        "sue@email.com",
                        "dhs",
                        "Honesty",
                        "10",
                        "8/7/2016 14:21:00",
                        "Great job!"});
            table4.AddRow(new string[] {
                        "john@email.com",
                        "tom@email.com",
                        "dhs",
                        "Excellence",
                        "70",
                        "8/4/2016 09:44:00",
                        "ipsum laurem"});
            table4.AddRow(new string[] {
                        "nikhil@email.com",
                        "jose@email.com",
                        "dhs",
                        "Respect",
                        "15",
                        "8/2/2016 19:04:00",
                        "don\'t know what i would do without you"});
            table4.AddRow(new string[] {
                        "joe@email.com",
                        "suresh@email.com",
                        "dhs",
                        "Integrity",
                        "30",
                        "8/2/2016 08:15:00",
                        "Great job!"});
#line 25
 testRunner.Then("I should see a list of my recognitions sorted most recent first:", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
