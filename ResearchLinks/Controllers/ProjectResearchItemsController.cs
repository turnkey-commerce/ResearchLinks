using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ResearchLinks.Data.Models;
using ResearchLinks.Data.Repository;
using ResearchLinks.app.DTO;

namespace ResearchLinks.Controllers
{
    [Authorize]
    public class ProjectResearchItemsController : ApiController
    {
        private readonly IResearchItemRepository _researchItemRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectResearchItemsController(IResearchItemRepository researchItemRepository, IProjectRepository projectRepository)
        {
            _researchItemRepository = researchItemRepository;
            _projectRepository = projectRepository;
        }

        // GET api/projects/4/researchItems
        public HttpResponseMessage Get(int projectId)
        {
            var researchItems = new List<ResearchItem>();
            var project = new Project();
            try {
                project = _projectRepository.GetByUser(projectId, User.Identity.Name);
                if (project == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Project not found for user " + User.Identity.Name + ".");
                }
                researchItems = _researchItemRepository.GetByUserAndProject(User.Identity.Name, projectId).ToList();
            } catch (Exception ex) {
                var error = new HttpError("Error getting research items: " + ex.Message) { { "Trace", ex.StackTrace } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, error);
            }

            var researchItemDto = new ResearchItemDto() { Meta = new ResearchItemMeta() { NumberResearchItems = researchItems.Count(), ProjectName = project.Name },
                 ResearchItems = researchItems };

            return Request.CreateResponse<ResearchItemDto>(HttpStatusCode.OK, researchItemDto);
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
