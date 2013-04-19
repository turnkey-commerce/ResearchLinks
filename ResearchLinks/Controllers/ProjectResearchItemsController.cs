using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ResearchLinks.Data.Models;
using ResearchLinks.Data.Repository;

namespace ResearchLinks.Controllers
{
    [Authorize]
    public class ProjectResearchItemsController : ApiController
    {
        private readonly IResearchItemRepository _researchItemRepository;

        public ProjectResearchItemsController(IResearchItemRepository researchItemRepository)
        {
            _researchItemRepository = researchItemRepository;
        }

        // GET api/projects/4/researchItems
        public HttpResponseMessage Get(int projectId)
        {
            var researchItems = new List<ResearchItem>();
            try {
                researchItems = _researchItemRepository.GetByUserAndProject(User.Identity.Name, projectId).ToList();
            } catch (Exception ex) {
                var error = new HttpError("Error getting research items: " + ex.Message) { { "Trace", ex.StackTrace } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, error);
            }
            return Request.CreateResponse<List<ResearchItem>>(HttpStatusCode.OK, researchItems);
        }

        // GET api/researchitems/5
        public string Get(int projectId, int researchItemId)
        {
            return "value";
        }

        // POST api/researchitems
        public void Post([FromBody]string value)
        {
        }

        // PUT api/researchitems/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/researchitems/5
        public void Delete(int id)
        {
        }
    }
}
