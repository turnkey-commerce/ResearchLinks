using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using ResearchLinks.SpecTests.Models;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace ResearchLinks.SpecTests
{
    [Binding]
    public class ProjectsAPISteps
    {
        private ProjectTestModel _projectTestModel = new ProjectTestModel();
        private string _responseContent;

        [Given(@"the following project inputs and authentication")]
        public void GivenTheFollowingProjectInputsAndAuthentication(Table table)
        {
            table.FillInstance<ProjectTestModel>(_projectTestModel);
        }

        [When(@"the client posts the inputs to the website")]
        public void WhenTheClientPostsTheInputsToTheWebsite()
        {

            var client = new HttpClient();
            var buffer = Encoding.ASCII.GetBytes(_projectTestModel.UserName + ":" + _projectTestModel.Password);
            var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(buffer));
            client.DefaultRequestHeaders.Authorization = authHeader;

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("Name", _projectTestModel.ProjectName));
            postData.Add(new KeyValuePair<string, string>("Description ", _projectTestModel.Description));
            postData.Add(new KeyValuePair<string, string>("UserName ", _projectTestModel.UserName));
            HttpContent content = new FormUrlEncodedContent(postData);

            var response = client.PostAsync("http://localhost:55301/api/projects", content).Result;

            
        }

        [Then(@"a (.*) status should be returned")]
        public void ThenAStatusShouldBeReturned(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"the client gets the project by ID")]
        public void WhenTheClientGetsTheProjectByID()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the response JSON should match the inputs")]
        public void ThenTheResponseJSONShouldMatchTheInputs()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
