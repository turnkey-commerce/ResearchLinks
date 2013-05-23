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
using ResearchLinks.DTO;

namespace ResearchLinks.Controllers.Version2
{
    [Authorize]
    public class ProjectsController : ProjectsControllerBase
    {
    
        //VERSION 2!

        // In version 2 we need to add an additional bool property IsUrgent on the post and put.
        // We will catch it in the controller since it is not required in the data model for backward compatibility in V1.

        // Constructor here to inject dependencies in concrete class.
        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // POST api/projects (Insert)
        public override HttpResponseMessage Post(Project project)
        {
            // Check for presence of V2 "IsUrgent" flag.
            if (project.IsUrgent == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The IsUrgent indicator is required.");
            }

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
        public override HttpResponseMessage Put(int id, Project project)
        {
            // Check for presence of V2 "IsUrgent" flag.
            if (project.IsUrgent == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The IsUrgent indicator is required.");
            }

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
                var error = new HttpError("Error updating project: " + ex.Message) { { "Trace", ex.StackTrace } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, error);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, currentProject);
            string uri = Url.Link("DefaultApi", new { id = id });
            response.Headers.Location = new Uri(uri);
            return response;
        }
    }
}
