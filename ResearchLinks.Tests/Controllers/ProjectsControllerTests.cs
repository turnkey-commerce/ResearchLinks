using System.Collections.Generic;
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
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using ResearchLinks.DTO;

namespace ResearchLinks.Tests.Controllers
{
    [TestFixture]
    public class ProjectsControllerTests
    {
        private RepositoryMocks _mockRepositories = new RepositoryMocks();
        private Mock<IProjectRepository> _projectRepository;

        [TestCase("V1")]
        [TestCase("V2")]
        public void ProjectsController_Class_Has_Authorization_Attribute(string version)
        {
            // Setup
            object[] attributes = null;
            if (version == "V1")
                attributes = typeof(ResearchLinks.Controllers.Version1.ProjectsController)
                    .GetCustomAttributes(typeof(AuthorizeAttribute), true);
            else if (version == "V2")
                attributes = typeof(ResearchLinks.Controllers.Version2.ProjectsController)
                    .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            //Assert
            Assert.Greater(attributes.Length, 0);
        }

        #region Get Projects Tests
        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_Projects_Returns_Expected_Projects_For_James(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Get, version);

            //Act
            var response = projectsController.Get();
            var responseContent = JsonConvert.DeserializeObject<ProjectDto>(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expecting an OK Status Code");
            Assert.AreEqual(2, responseContent.Projects.Count);
            Assert.AreEqual(2, responseContent.Meta.NumberProjects);
            Assert.AreEqual("james", responseContent.Projects[0].UserName);
            Assert.AreEqual("Test Project 1", responseContent.Projects[0].Name);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_Projects_Returns_Expected_Projects_For_John(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            var projectsController = SetupController(_projectRepository.Object, "john", HttpMethod.Get, version);

            //Act
            var response = projectsController.Get();
            var responseContent = JsonConvert.DeserializeObject<ProjectDto>(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expecting an OK Status Code");
            Assert.AreEqual(1, responseContent.Projects.Count);
            Assert.AreEqual(1, responseContent.Meta.NumberProjects);
            Assert.AreEqual("john", responseContent.Projects[0].UserName);
            Assert.AreEqual("Test Project 3", responseContent.Projects[0].Name);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_Projects_Database_Exception_Returns_Error(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Exception);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Get, version);

            //Act
            var response = projectsController.Get();
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error getting projects: Database exception!", (string)errorMessage.Message);
        } 
        #endregion

        #region Get Project Tests
        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_Project_By_ProjectId_Returns_Expected_Project_For_James(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Get, version);

            //Act
            var response = projectsController.Get(1);
            var project = JsonConvert.DeserializeObject<Project>(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expecting an OK Status Code");
            Assert.AreEqual(1, project.ProjectId);
            Assert.AreEqual("james", project.UserName);
            Assert.AreEqual("Test Project 1", project.Name);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_Project_By_ProjectId_Returns_Null_Project_For_John(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            var projectsController = SetupController(_projectRepository.Object, "john", HttpMethod.Get, version);

            //Act
            var response = projectsController.Get(1);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expecting a Not Found Status Code");
            Assert.AreEqual("Project not found for user john.", (string)errorMessage.Message);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_Project_By_ProjectId_Database_Exception_Returns_Error(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Exception);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Get, version);

            //Act
            var response = projectsController.Get(1);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error getting project: Database exception!", (string)errorMessage.Message);
        } 
        #endregion

        #region Post Project Tests
        [TestCase("V1")]
        [TestCase("V2")]
        public void Post_Project_Returns_Expected_Header(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Post, version);
            var inputProject = new Project() { ProjectId = 1, Name = "Insert Test", UserName = "james", Description = "Insert Test Description", IsUrgent = true };

            //Act
            var response = projectsController.Post(inputProject);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "Expecting a Created Status Code");
            Assert.AreEqual("http://localhost/api/projects/1", response.Headers.Location.ToString());
        }

        [TestCase("V2")]
        public void Post_Project_When_IsUrgent_Missing_Returns_Bad_Request(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Post, version);
            var inputProject = new Project() { ProjectId = 1, Name = "Insert Test", UserName = "james", Description = "Insert Test Description" };

            //Act
            var response = projectsController.Post(inputProject);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expecting a Bad Request Status Code");
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Post_Project_Database_Exception_Returns_Error(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Exception);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Post, version);
            var inputProject = new Project() { ProjectId = 1, Name = "Insert Test", UserName = "james", Description = "Insert Test Description", IsUrgent = true };

            //Act
            var response = projectsController.Post(inputProject);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error inserting project: Database exception!", (string)errorMessage.Message);
        } 
        #endregion

        #region Put Project Tests
        [TestCase("V1")]
        [TestCase("V2")]
        public void Put_Project_Returns_Expected_Header_For_James(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Put, version);
            var inputProject = new Project() { ProjectId = 1, Name = "Update Test 1", UserName = "james", Description = "Insert Test Description", IsUrgent = true };

            //Act
            var response = projectsController.Put(1, inputProject);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expecting an OK Status Code");
            Assert.AreEqual("http://localhost/api/projects/1", response.Headers.Location.ToString());
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Put_James_Project_Returns_Null_Project_For_John(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            var projectsController = SetupController(_projectRepository.Object, "john", HttpMethod.Put, version);
            var inputProject = new Project() { ProjectId = 1, Name = "Update Test 1", UserName = "john", Description = "Insert Test Description", IsUrgent = true };

            //Act
            var response = projectsController.Put(1, inputProject);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expecting a Not Found Status Code");
            Assert.AreEqual("Project not found for user john.", (string)errorMessage.Message);
        }

        [TestCase("V2")]
        public void Put_Project_When_IsUrgent_Missing_Returns_Bad_Request(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Exception);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Put, version);
            var inputProject = new Project() { ProjectId = 1, Name = "Update Test 1", UserName = "james", Description = "Insert Test Description" };

            //Act
            var response = projectsController.Put(1, inputProject);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expecting a Bad Request Status Code");
        } 

        [TestCase("V1")]
        [TestCase("V2")]
        public void Put_Project_Database_Exception_Returns_Error(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Exception);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Put, version);
            var inputProject = new Project() { ProjectId = 1, Name = "Update Test 1", UserName = "james", Description = "Insert Test Description", IsUrgent = true };

            //Act
            var response = projectsController.Put(1, inputProject);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error updating project: Database exception!", (string)errorMessage.Message);
        } 
        #endregion

        #region Delete Project Tests
        [TestCase("V1")]
        [TestCase("V2")]
        public void Delete_Project_Returns_Expected_Header_For_James(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Delete, version);

            //Act
            var response = projectsController.Delete(1);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, "Expecting No Content Status Code");
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Delete_Project_Returns_Null_Project_For_John(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            var projectsController = SetupController(_projectRepository.Object, "john", HttpMethod.Delete, version);

            //Act
            var response = projectsController.Delete(1);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expecting a Not Found Status Code");
            Assert.AreEqual("Project not found for user john.", (string)errorMessage.Message);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Delete_Project_Database_Exception_Returns_Error(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Exception);
            var projectsController = SetupController(_projectRepository.Object, "james", HttpMethod.Delete, version);

            //Act
            var response = projectsController.Delete(1);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error deleting project: Database exception!", (string)errorMessage.Message);
        } 
        #endregion

        private dynamic SetupController(IProjectRepository mockRepository, string userName, HttpMethod method, string version)
        {
            dynamic projectsController = null;
            
            if (version == "V1") 
            {
                projectsController = new ResearchLinks.Controllers.Version1.ProjectsController(mockRepository);
            } 
            else if (version == "V2") 
            {
                projectsController = new ResearchLinks.Controllers.Version2.ProjectsController(mockRepository);
            }
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();
            user.Setup(x => x.Identity).Returns(identity.Object);
            identity.Setup(x => x.Name).Returns(userName);
            Thread.CurrentPrincipal = user.Object;

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(method, "http://localhost/api/projects");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "projects" } });

            projectsController.ControllerContext = new HttpControllerContext(config, routeData, request);
            projectsController.Request = request;
            projectsController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            projectsController.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            
            return projectsController;
        }
    }
}
