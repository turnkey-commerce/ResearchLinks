using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ResearchLinks.Data.Models;
using ResearchLinks.Data.Repository;
using ResearchLinks.Tests.Mocks;
using Moq;
using ResearchLinks.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Net;
using System.Security.Principal;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ResearchLinks.Tests.Controllers
{
    [TestFixture]
    public class ProjectsControllerTests
    {
        private RepositoryMocks _mockRepositories = new RepositoryMocks();
        private Mock<IProjectRepository> _projectRepository;

        [Test]
        public void Get_Projects_Returns_Expected_Projects_For_James()
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(false);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Get);
            
            //Act
            var response = projectsController.Get();
            var projects = JsonConvert.DeserializeObject<List<Project>>(response.Content.ReadAsStringAsync().Result);
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expecting an OK Status Code");
            Assert.AreEqual(2, projects.Count);
            Assert.AreEqual("james", projects[0].UserName);
            Assert.AreEqual("Test Project 1", projects[0].Name);      
        }

        [Test]
        public void Get_Projects_Returns_Expected_Projects_For_John()
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(false);
            var projectsController = SetupController(_projectRepository.Object, "john", HttpMethod.Get);

            //Act
            var response = projectsController.Get();
            var projects = JsonConvert.DeserializeObject<List<Project>>(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expecting an OK Status Code");
            Assert.AreEqual(1, projects.Count);
            Assert.AreEqual("john", projects[0].UserName);
            Assert.AreEqual("Test Project 3", projects[0].Name);
        }

        [Test]
        public void Get_Projects_Database_Exception_Returns_Error()
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(true);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Get);

            //Act
            var response = projectsController.Get();
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error getting projects: Database exception!", (string) errorMessage.Message);
        }

        [Test]
        public void Get_Project_By_ProjectId_Returns_Expected_Project_For_James()
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(false);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Get);

            //Act
            var response = projectsController.Get(1);
            var project = JsonConvert.DeserializeObject<Project>(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expecting an OK Status Code");
            Assert.AreEqual(1, project.ProjectId);
            Assert.AreEqual("james", project.UserName);
            Assert.AreEqual("Test Project 1", project.Name);
        }

        [Test]
        public void Get_Project_By_ProjectId_Returns_Null_Project_For_John()
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(false);
            var projectsController = SetupController(_projectRepository.Object, "john", HttpMethod.Get);

            //Act
            var response = projectsController.Get(1);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expecting a Not Found Status Code");
            Assert.AreEqual("Project not found for user john.", (string)errorMessage.Message);
        }

        [Test]
        public void Get_Project_By_ProjectId_Database_Exception_Returns_Error()
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(true);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Get);

            //Act
            var response = projectsController.Get(1);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error getting project: Database exception!", (string)errorMessage.Message);
        }

        private ProjectsController SetupController(IProjectRepository mockRepository, string userName, HttpMethod method)
        {
            var projectsController = new ProjectsController(mockRepository);
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();
            user.Setup(x => x.Identity).Returns(identity.Object);
            identity.Setup(x => x.Name).Returns(userName);
            Thread.CurrentPrincipal = user.Object;
            projectsController.Request = new HttpRequestMessage(method, "http://researchlinks.com/projects");
            projectsController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            projectsController.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());
            return projectsController;
        }
    }
}
