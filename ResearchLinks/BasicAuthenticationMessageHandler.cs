using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Http;
using System.Net;

namespace ResearchLinks
{
    public class BasicAuthenticationMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var authHeader = request.Headers.Authorization;

            if (authHeader == null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            if (authHeader.Scheme != "Basic")
            {
                return base.SendAsync(request, cancellationToken);
            }

            var encodedUserPass = authHeader.Parameter.Trim();
            var userPass = Encoding.ASCII.GetString(Convert.FromBase64String(encodedUserPass));
            var parts = userPass.Split(":".ToCharArray());
            var username = parts[0];
            var password = parts[1];


            if (username == "" || password == "")
            {
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                if (!Membership.ValidateUser(username, password))
                {
                    return base.SendAsync(request, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                var response = request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                return Task<HttpResponseMessage>.Factory.StartNew(() => response);
            }

            try
            {
                var identity = new GenericIdentity(username, "Basic");
                string[] roles = Roles.Provider.GetRolesForUser(username);
                var principal = new GenericPrincipal(identity, roles);
                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
            }
            catch (Exception ex)
            {
                var response = request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                return Task<HttpResponseMessage>.Factory.StartNew(() => response);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}