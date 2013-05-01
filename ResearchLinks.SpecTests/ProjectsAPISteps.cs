using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using ResearchLinks.SpecTests.Models;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Collections.Generic;
using NUnit.Framework;
using System.Net;
using Newtonsoft.Json;
using ResearchLinks.Data.Models;
using System.Reflection;
using ResearchLinks.SpecTests.Helpers;

namespace ResearchLinks.SpecTests
{
    [Binding]
    public class ProjectsAPISteps
    {
        private ProjectTestModel _projectTestModel = new ProjectTestModel();
        private HttpResponseMessage _responseContent;
        private Project _projectSaved;

        [Given(@"the following project inputs and authentication")]
        public void GivenTheFollowingProjectInputsAndAuthentication(Table table)
        {
            table.FillInstance<ProjectTestModel>(_projectTestModel);
        }

        [When(@"the client posts the inputs to the website")]
        public void WhenTheClientPostsTheInputsToTheWebsite()
        {

            var client = StepHelpers.SetupHttpClient(_projectTestModel.UserName, _projectTestModel.Password);

            var postData = StepHelpers.SetPostData<ProjectTestModel>(_projectTestModel);
            HttpContent content = new FormUrlEncodedContent(postData);

            _responseContent = client.PostAsync("http://localhost:55301/api/projects", content).Result;
            client.Dispose();
        }

        [Then(@"a (.*) status should be returned")]
        public void ThenAStatusShouldBeReturned(string statusCode)
        {
            Assert.AreEqual(statusCode, _responseContent.StatusCode.ToString());
        }

        [When(@"the client gets the project by header location")]
        public void WhenTheClientGetsTheProjectByHeaderLocation()
        {
            var client = StepHelpers.SetupHttpClient(_projectTestModel.UserName, _projectTestModel.Password);
            _responseContent = client.GetAsync(_responseContent.Headers.Location).Result;
            _projectSaved = JsonConvert.DeserializeObject<Project>(_responseContent.Content.ReadAsStringAsync().Result);
        }

        [Then(@"the saved project matches the inputs")]
        public void ThenTheSavedProjectMatchesTheInputs()
        {
            Assert.AreEqual(_projectTestModel.Name, _projectSaved.Name);
            Assert.AreEqual(_projectTestModel.Description, _projectSaved.Description);
            Assert.AreEqual(_projectTestModel.UserName, _projectSaved.UserName);
        }
    }
}
