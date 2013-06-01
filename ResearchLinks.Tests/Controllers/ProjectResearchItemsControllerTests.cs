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
    public class ProjectResearchItemsControllerTests
    {
        private RepositoryMocks _mockRepositories = new RepositoryMocks();
        private Mock<IProjectRepository> _projectRepository;
        private Mock<IResearchItemRepository> _researchItemRepository;

        [TestCase("V1")]
        [TestCase("V2")]
        public void ProjectsController_Class_Has_Authorization_Attribute(string version)
        {
            // Setup
            object[] attributes = null;
            if (version == "V1")
                attributes = typeof(ResearchLinks.Controllers.Version1.ProjectResearchItemsController)
                    .GetCustomAttributes(typeof(AuthorizeAttribute), true);
            else if (version == "V2")
                attributes = typeof(ResearchLinks.Controllers.Version2.ProjectResearchItemsController)
                    .GetCustomAttributes(typeof(AuthorizeAttribute), true);
            //Assert
            Assert.Greater(attributes.Length, 0);
        }

        #region Get ResearchItems Tests
        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_ResearchItems_Returns_Expected_ResearchItems_For_James(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Get, version);

            //Act
            var response = researchItemsController.Get(1);
            var responseContent = JsonConvert.DeserializeObject<ResearchItemDto>(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expecting an OK Status Code");
            Assert.AreEqual(2, responseContent.ResearchItems.Count);
            Assert.AreEqual(2, responseContent.Meta.NumberResearchItems);
            Assert.AreEqual("Test Project 1", responseContent.Meta.ProjectName);
            Assert.AreEqual("james", responseContent.ResearchItems[0].UserName);
            Assert.AreEqual("Test Research Item 1", responseContent.ResearchItems[0].Subject);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_ResearchItems_Returns_Expected_ResearchItems_For_John(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "john", HttpMethod.Get, version);

            //Act
            var response = researchItemsController.Get(3);
            var responseContent = JsonConvert.DeserializeObject<ResearchItemDto>(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expecting an OK Status Code");
            Assert.AreEqual(1, responseContent.ResearchItems.Count);
            Assert.AreEqual(1, responseContent.Meta.NumberResearchItems);
            Assert.AreEqual("Test Project 3", responseContent.Meta.ProjectName);
            Assert.AreEqual("john", responseContent.ResearchItems[0].UserName);
            Assert.AreEqual("Test Research Item 3", responseContent.ResearchItems[0].Subject);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_ResearchItems_Returns_Not_Found_For_John_With_Wrong_ProjectId(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "john", HttpMethod.Get, version);

            //Act
            var response = researchItemsController.Get(1);
            var responseContent = JsonConvert.DeserializeObject<ResearchItemDto>(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expecting a Not Found Status Code");
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_ResearchItems_Database_Exception_Returns_Error(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Exception);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "john", HttpMethod.Get, version);

            //Act
            var response = researchItemsController.Get(3);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error getting research items: Database exception!", (string)errorMessage.Message);
        } 
        #endregion

        #region Get ResearchItem Tests
        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_ResearchItem_By_ResearchItem_And_ProjectId_Returns_Expected_ResearchItem_For_James(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Get, version);

            //Act
            var response = researchItemsController.Get(1, 1);
            var responseContent = JsonConvert.DeserializeObject<ResearchItemDto>(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expecting an OK Status Code");
            Assert.AreEqual(1, responseContent.ResearchItems.Count);
            Assert.AreEqual(1, responseContent.Meta.NumberResearchItems);
            Assert.AreEqual("Test Project 1", responseContent.Meta.ProjectName);
            Assert.AreEqual(1, responseContent.ResearchItems[0].ProjectId);
            Assert.AreEqual("james", responseContent.ResearchItems[0].UserName);
            Assert.AreEqual("Test Research Item 1", responseContent.ResearchItems[0].Subject);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_ResearchItem_By_ResearchItem_And_ProjectId_Returns_Not_Found_With_Wrong_ProjectId_For_James(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Get, version);

            //Act
            var response = researchItemsController.Get(3,1);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expecting a Not Found Status Code");
            Assert.AreEqual("Project not found for user james.", (string)errorMessage.Message);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_ResearchItem_By_ResearchItem_And_ProjectId_Returns_Not_Found_With_Wrong_ResearchItemId_For_James(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Get, version);

            //Act
            var response = researchItemsController.Get(1, 3);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expecting a Not Found Status Code");
            Assert.AreEqual("Research item not found for user james.", (string)errorMessage.Message);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Get_ResearchItem_By_ResearchItem_And_ProjectId_Database_Exception_Returns_Error(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Exception);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Get, version);

            //Act
            var response = researchItemsController.Get(1, 1);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error getting research item: Database exception!", (string)errorMessage.Message);
        }
        #endregion

        #region Post ResearchItem Tests
        [TestCase("V1")]
        [TestCase("V2")]
        public void Post_ResearchItem_Returns_Expected_Header(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Post, version);
            var inputResearchItem = new ResearchItem() { ResearchItemId = 1, ProjectId = 1, Subject = "Insert Test", UserName = "james", Description = "Insert Test Description" };

            //Act
            var response = researchItemsController.Post(1, inputResearchItem);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "Expecting a Created Status Code");
            Assert.AreEqual("http://localhost/api/projects/1/researchItems/1", response.Headers.Location.ToString());
        }


        [TestCase("V1")]
        [TestCase("V2")]
        public void Post_ResearchItem_Returns_NotFound_Error_With_Wrong_ProjectId_For_James(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Post, version);
            var inputResearchItem = new ResearchItem() { ResearchItemId = 1, ProjectId = 3, Subject = "Insert Test", UserName = "james", Description = "Insert Test Description" };

            //Act
            var response = researchItemsController.Post(3, inputResearchItem);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expecting a Not Found Status Code");
            Assert.AreEqual("Project not found for user james.", (string)errorMessage.Message);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Post_Project_Database_Exception_Returns_Error(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Exception);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Post, version);
            var inputResearchItem = new ResearchItem() { ResearchItemId = 1, ProjectId = 1, Subject = "Insert Test", UserName = "james", Description = "Insert Test Description" };

            //Act
            var response = researchItemsController.Post(1, inputResearchItem);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error inserting research item: Database exception!", (string)errorMessage.Message);
        }
        #endregion

        #region Put ResearchItem Tests
        [TestCase("V1")]
        [TestCase("V2")]
        public void Put_ResearchItem_Returns_Expected_Header_For_James(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Put, version);
            var inputResearchItem = new ResearchItem() { ResearchItemId = 1, ProjectId = 1, Subject = "Update Test 1", UserName = "james", Description = "Insert Test Description" };

            //Act
            var response = researchItemsController.Put(1, 1, inputResearchItem);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expecting an OK Status Code");
            Assert.AreEqual("http://localhost/api/projects/1/researchItems/1", response.Headers.Location.ToString());
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Put_James_ResearchItem_Returns_Null_ResearchItem_For_John(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "john", HttpMethod.Put, version);
            var inputResearchItem = new ResearchItem() { ResearchItemId = 1, ProjectId = 3, Subject = "Update Test 1", UserName = "john", Description = "Insert Test Description" };

            //Act
            var response = researchItemsController.Put(3, 1, inputResearchItem);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expecting a Not Found Status Code");
            Assert.AreEqual("Research item not found for user john.", (string)errorMessage.Message);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Put_ResearchItem_Database_Exception_Returns_Error(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Exception);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Put, version);
            var inputResearchItem = new ResearchItem() { ResearchItemId = 1, ProjectId = 1, Subject = "Update Test 1", UserName = "james", Description = "Insert Test Description" };

            //Act
            var response = researchItemsController.Put(1, 1, inputResearchItem);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error updating research item: Database exception!", (string)errorMessage.Message);
        }
        #endregion

        #region Delete ResearchItem Tests
        [TestCase("V1")]
        [TestCase("V2")]
        public void Delete_ResearchItem_Returns_Expected_Header_For_James(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Delete, version);

            //Act
            var response = researchItemsController.Delete(1,1);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, "Expecting No Content Status Code");
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Delete_James_ResearchItem_Returns_Null_ResearchItem_For_John(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Normal);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "john", HttpMethod.Delete, version);

            //Act
            var response = researchItemsController.Delete(3, 1);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expecting a Not Found Status Code");
            Assert.AreEqual("Research item not found for user john.", (string)errorMessage.Message);
        }

        [TestCase("V1")]
        [TestCase("V2")]
        public void Delete_Project_Database_Exception_Returns_Error(string version)
        {
            //Setup
            _projectRepository = _mockRepositories.GetProjectsRepository(ReturnType.Normal);
            _researchItemRepository = _mockRepositories.GetResearchItemsRepository(ReturnType.Exception);
            var researchItemsController = SetupController(_researchItemRepository.Object, _projectRepository.Object, "james", HttpMethod.Delete, version);

            //Act
            var response = researchItemsController.Delete(1,1);
            dynamic errorMessage = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode, "Expecting an Internal Server Error Status Code");
            Assert.AreEqual("Error deleting research item: Database exception!", (string)errorMessage.Message);
        }
        #endregion

        private dynamic SetupController(IResearchItemRepository mockResearchItemRepository, IProjectRepository mockProjectRepository, string userName, HttpMethod method, string version)
        {
            dynamic projectResearchItemsController = null;

            if (version == "V1") {
                projectResearchItemsController = new ResearchLinks.Controllers.Version1.ProjectResearchItemsController(mockResearchItemRepository, mockProjectRepository);
            } else if (version == "V2") {
                projectResearchItemsController = new ResearchLinks.Controllers.Version2.ProjectResearchItemsController(mockResearchItemRepository, mockProjectRepository);
            }
            
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();
            user.Setup(x => x.Identity).Returns(identity.Object);
            identity.Setup(x => x.Name).Returns(userName);
            Thread.CurrentPrincipal = user.Object;

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(method, "http://localhost/api/projects/1/researchItems");
            var route = config.Routes.MapHttpRoute("ProjectResearchItemsApi", "api/projects/{projectId}/researchItems/{researchItemId}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "projectResearchItems" } });

            projectResearchItemsController.ControllerContext = new HttpControllerContext(config, routeData, request);
            projectResearchItemsController.Request = request;
            projectResearchItemsController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            projectResearchItemsController.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

            return projectResearchItemsController;
        }
    }
}
