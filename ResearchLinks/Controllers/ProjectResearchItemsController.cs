using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ResearchLinks.Data.Models;
using ResearchLinks.Data.Repository;
using ResearchLinks.DTO;

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

        // GET /api/projects/4/researchItems
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

        // GET /api/projects/4/researchitems/5
        public HttpResponseMessage Get(int projectId, int researchItemId)
        {
            var researchItems = new List<ResearchItem>();
            var project = new Project();
            try
            {
                project = _projectRepository.GetByUser(projectId, User.Identity.Name);
                if (project == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Project not found for user " + User.Identity.Name + ".");
                }
                var researchItem = _researchItemRepository.GetByUser(researchItemId, User.Identity.Name);
                if (researchItem == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Research item not found for user " + User.Identity.Name + ".");
                }
                researchItems.Add(researchItem);
            }
            catch (Exception ex)
            {
                var error = new HttpError("Error getting research item: " + ex.Message) { { "Trace", ex.StackTrace } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, error);
            }

            var researchItemDto = new ResearchItemDto()
            {
                Meta = new ResearchItemMeta() { NumberResearchItems = researchItems.Count(), ProjectName = project.Name },
                ResearchItems = researchItems
            };

            return Request.CreateResponse<ResearchItemDto>(HttpStatusCode.OK, researchItemDto);
        }

        // POST /api/projects/4/researchItems
        public HttpResponseMessage Post(int projectId, ResearchItem researchItem)
        {
            try {
                // Make sure it is being inserted to a project owned by the user.
                var project = _projectRepository.GetByUser(projectId, User.Identity.Name);
                if (project == null) {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Project not found for user " + User.Identity.Name + ".");
                }
                researchItem.ProjectId = projectId;
                researchItem.UserName = User.Identity.Name;
                researchItem.DateCreated = DateTime.Now;
                researchItem.DateUpdated = DateTime.Now;
                _researchItemRepository.Insert(researchItem);
                _researchItemRepository.Commit();
            } catch (Exception ex) {
                var error = new HttpError("Error inserting research item: " + ex.Message) { { "Trace", ex.StackTrace } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, error);
            }
            var response = Request.CreateResponse(HttpStatusCode.Created, researchItem);
            string uri = Url.Link("ProjectResearchItemsApi", new { projectId = projectId, researchItemId = researchItem.ResearchItemId });
            response.Headers.Location = new Uri(uri);
            return response;

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
