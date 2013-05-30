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
using ResearchLinks.DTO;

namespace ResearchLinks.SpecTests
{
    [Binding]
    public class ProjectsAPISteps
    {
        private ProjectTestModel _projectTestModel = new ProjectTestModel();
        private HttpResponseMessage _responseContent;
        private Project _projectSaved;
        private ProjectDto _projectDto;

        [Given(@"the following project inputs and authentication")]
        public void GivenTheFollowingProjectInputsAndAuthentication(Table table)
        {
            // First initialize back in case of resuse in the same run.
            _projectTestModel.IsUrgent = null;
            _projectTestModel.Name = "";
            _projectTestModel.Description = "";
            table.FillInstance<ProjectTestModel>(_projectTestModel);
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
            client.Dispose();
        }

        [When(@"the client gets the project by ID")]
        public void WhenTheClientGetsTheProjectByID()
        {
            var client = StepHelpers.SetupHttpClient(_projectTestModel.UserName, _projectTestModel.Password);
            _responseContent = client.GetAsync("http://localhost:55301/api/v1/projects/" + _projectSaved.ProjectId).Result;
            _projectSaved = JsonConvert.DeserializeObject<Project>(_responseContent.Content.ReadAsStringAsync().Result);
            client.Dispose();
        }

        [Then(@"the saved project matches the inputs for V(.*)")]
        public void ThenTheSavedProjectMatchesTheInputs(int version)
        {
            Assert.AreEqual(_projectTestModel.Name, _projectSaved.Name);
            Assert.AreEqual(_projectTestModel.Description, _projectSaved.Description);
            Assert.AreEqual(_projectTestModel.UserName, _projectSaved.UserName);
            if (version == 2) 
            {
                Assert.AreEqual(_projectTestModel.IsUrgent, _projectSaved.IsUrgent);
            }
        }

        [When(@"the client gets all projects")]
        public void WhenTheClientGetsAllProjects()
        {
            var client = StepHelpers.SetupHttpClient(_projectTestModel.UserName, _projectTestModel.Password);

            _responseContent = client.GetAsync("http://localhost:55301/api/v1/projects").Result;
            _projectDto = JsonConvert.DeserializeObject<ProjectDto>(_responseContent.Content.ReadAsStringAsync().Result);
            client.Dispose();
        }

        [Then(@"the saved project should be in the list")]
        public void ThenTheSavedProjectShouldBeInTheList()
        {
            Assert.AreEqual(1, _projectDto.Projects.Count);
            Assert.AreEqual(_projectTestModel.Name, _projectDto.Projects[0].Name);
            Assert.AreEqual(_projectTestModel.Description, _projectDto.Projects[0].Description);
            Assert.AreEqual(_projectTestModel.UserName, _projectDto.Projects[0].UserName);
        }

        [When(@"the client issues delete for the save project")]
        public void WhenTheClientIssuesDeleteForTheSaveProject()
        {
            var client = StepHelpers.SetupHttpClient(_projectTestModel.UserName, _projectTestModel.Password);

            _responseContent = client.DeleteAsync("http://localhost:55301/api/v1/projects/" + _projectSaved.ProjectId).Result;
            client.Dispose();
        }

        [When(@"the client posts the inputs to the website for V(.*)")]
        public void WhenTheClientPostsTheInputsToTheWebsiteForV(int version)
        {
            var client = StepHelpers.SetupHttpClient(_projectTestModel.UserName, _projectTestModel.Password);

            var postData = StepHelpers.SetPostData<ProjectTestModel>(_projectTestModel);
            HttpContent content = new FormUrlEncodedContent(postData);

            _responseContent = client.PostAsync("http://localhost:55301/api/v" + version.ToString() + "/projects", content).Result;
            client.Dispose();
        }

        [When(@"the client puts the inputs to the website for V(.*)")]
        public void WhenTheClientPutsTheInputsToTheWebsiteForV(int version)
        {
            var client = StepHelpers.SetupHttpClient(_projectTestModel.UserName, _projectTestModel.Password);

            var postData = StepHelpers.SetPostData<ProjectTestModel>(_projectTestModel);
            HttpContent content = new FormUrlEncodedContent(postData);

            _responseContent = client.PutAsync("http://localhost:55301/api/v" + version.ToString() + "/projects/" + _projectSaved.ProjectId, content).Result;
            client.Dispose();
        }
    }
}
