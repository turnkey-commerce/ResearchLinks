using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ResearchLinks.Controllers
{
    public class LoginController : ApiController
    {
        [Authorize]
        public HttpResponseMessage Post()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
