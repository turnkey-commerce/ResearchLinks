using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ResearchLinks.Data.Models;
using ResearchLinks.Data;
using System.Web.Http.ModelBinding;
using ResearchLinks.Filters;
using System.Data;
using ResearchLinks.Data.Repository;

namespace ResearchLinks.Controllers
{
    [Authorize]
    public class ProjectsController : ApiController
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // GET api/projects
        public HttpResponseMessage Get()
        {
            var projects = new List<Project>();
            try
            {
                projects = _projectRepository.GetByUser(User.Identity.Name).ToList();       
            }
            catch (Exception ex)
            {
                var error = new HttpError("Error getting projects: " + ex.Message) { { "Trace", ex.StackTrace } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, error);
            }
            return Request.CreateResponse<List<Project>>(HttpStatusCode.OK, projects);
        }

        // GET api/projects/5 (Detail)
        public HttpResponseMessage Get(int id)
        {
            var project = new Project();
            try
            {
                // Verify that the user is the owner
                project = _projectRepository.GetByUser(id, User.Identity.Name);
                if (project == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Project not found for user " + User.Identity.Name + ".");
                }
            }
            catch (Exception ex)
            {
                var error = new HttpError("Error getting project: " + ex.Message) { { "Trace", ex.StackTrace } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, error);
            }
            return Request.CreateResponse<Project>(HttpStatusCode.OK, project);
        }


        // POST api/projects (Insert)
        public HttpResponseMessage Post(Project project)
        {
            try
            {
                project.UserName = User.Identity.Name;
                project.DateCreated = DateTime.Now;
                project.DateUpdated = DateTime.Now;
                _projectRepository.Insert(project);
                _projectRepository.Commit();
            }
            catch (Exception ex)
            {
                var error = new HttpError("Error inserting project: " + ex.Message) { { "Trace", ex.StackTrace } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, error);
            }
            var response = Request.CreateResponse(HttpStatusCode.Created, project);
            string uri = Url.Link("DefaultApi", new { id = project.ProjectId });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // PUT api/projects/5  (Update)
        public HttpResponseMessage Put(int id, Project project)
        {
            var currentProject = new Project();
            try
            {
                // Verify that the user is the owner
                currentProject = _projectRepository.GetByUser(id, User.Identity.Name);
                if (currentProject == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Project not found for user " + User.Identity.Name + ".");
                }
                currentProject.DateUpdated = DateTime.Now;
                currentProject.Name = project.Name;
                currentProject.Description = project.Description;
                _projectRepository.Commit();
            }
            catch (Exception ex)
            {
                var error = new HttpError("Error updating project: " + ex.Message) {{"Trace", ex.StackTrace}};
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, error);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, currentProject);
            string uri = Url.Link("DefaultApi", new { id = id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // DELETE api/projects/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                // Verify that the user is the owner
                var currentProject = _projectRepository.GetByUser(id, User.Identity.Name);
                if (currentProject == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Project not found for user " + User.Identity.Name + ".");
                }
                _projectRepository.Delete(currentProject);
                _projectRepository.Commit();
            }
            catch (Exception ex)
            {
                var error = new HttpError("Error deleting project: " + ex.Message) { { "Trace", ex.StackTrace } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, error);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
