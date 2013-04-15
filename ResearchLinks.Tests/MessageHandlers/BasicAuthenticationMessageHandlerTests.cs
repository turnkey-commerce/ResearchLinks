using System;
using System.Text;
using NUnit.Framework;
using System.Net.Http;
using System.Threading;
using System.Net;
using ResearchLinks.Tests.Mocks;
using System.Net.Http.Headers;
using ResearchLinks.Services;
namespace ResearchLinks.Tests.MessageHandlers
{
    [TestFixture]
    class BasicAuthenticationMessageHandlerTests
    {
        [Test]
        public void MessageHandler_Should_Not_Add_Principal_If_Auth_Header_Missing()
        {
            //Setup
            MockInnerHandler innerhandler = new MockInnerHandler();
            innerhandler.Message = new HttpResponseMessage(HttpStatusCode.OK);

            HttpMessageInvoker client = new HttpMessageInvoker(new BasicAuthenticationMessageHandler { InnerHandler = innerhandler, MembershipService = new MockMembershipService(), RoleService = new MockRoleService() });
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/projects");

            //Action
            HttpResponseMessage message = client.SendAsync(requestMessage, new CancellationToken(false)).Result;

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, message.StatusCode);
            Assert.AreEqual(String.Empty, Thread.CurrentPrincipal.Identity.Name);
        }

        [Test]
        public void MessageHandler_Should_Add_Principal_If_Auth_Header_Included()
        {
            //Setup
            MockInnerHandler innerhandler = new MockInnerHandler();
            innerhandler.Message = new HttpResponseMessage(HttpStatusCode.OK);

            HttpMessageInvoker client = new HttpMessageInvoker(new BasicAuthenticationMessageHandler { InnerHandler = innerhandler, MembershipService = new MockMembershipService(), RoleService = new MockRoleService() });
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/projects");

            var usernamepwd = Convert.ToBase64String(Encoding.UTF8.GetBytes("james:goodPassword"));
            var authorization = new AuthenticationHeaderValue("Basic", usernamepwd);
            requestMessage.Headers.Authorization = authorization;

            //Action
            HttpResponseMessage message = client.SendAsync(requestMessage, new CancellationToken(false)).Result;

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, message.StatusCode);
            Assert.AreEqual("james", Thread.CurrentPrincipal.Identity.Name);
        }
    }
}
