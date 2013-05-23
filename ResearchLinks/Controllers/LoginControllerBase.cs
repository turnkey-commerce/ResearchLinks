using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ResearchLinks.Controllers
{
    public abstract class LoginControllerBase : ApiController
    {
        [Authorize]
        public virtual HttpResponseMessage Post()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public virtual HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
