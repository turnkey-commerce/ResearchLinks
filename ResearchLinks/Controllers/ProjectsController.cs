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

namespace ResearchLinks.Controllers
{
    [Authorize]
    public class ProjectsController : ApiController
    {
        // GET api/projects
        public HttpResponseMessage Get()
        {
            var projects = new List<Project>();
            using (var db = new ResearchLinksContext())
            {
                try
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    projects = db.Projects.Where(p => p.UserName == User.Identity.Name).ToList();
                    
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Error getting projects: " + ex.Message),
                        ReasonPhrase = "Database Exception"
                    };
                }
            }
            return Request.CreateResponse<List<Project>>(HttpStatusCode.OK, projects);
        }

        // GET api/projects/5 (Detail)
        public HttpResponseMessage Get(int id)
        {
            var project = new Project();
            using (var db = new ResearchLinksContext())
            {
                try
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    // Verify that the user is the owner
                    project = db.Projects.Where(p => p.ProjectId == id && p.UserName.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
                    if (project == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("Project not found for user " + User.Identity.Name + ".")
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Error getting project: " + ex.Message),
                        ReasonPhrase = "Database Exception"
                    };
                }
            }
            return Request.CreateResponse<Project>(HttpStatusCode.OK, project);
        }


        // POST api/projects (Insert)
        public HttpResponseMessage Post(Project project)
        {
            using (var db = new ResearchLinksContext())
            {
                db.Configuration.ProxyCreationEnabled = false;
                try
                {
                    project.UserName = User.Identity.Name;
                    project.DateCreated = DateTime.Now;
                    project.DateUpdated = DateTime.Now;
                    db.Projects.Add(project);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent("Error inserting project: " + ex.Message),
                            ReasonPhrase = "Database Exception"
                        };
                }
            }
            var response = Request.CreateResponse(HttpStatusCode.Created, project);
            string uri = Url.Link("DefaultApi", new { id = project.ProjectId });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // PUT api/projects  (Update)
        public HttpResponseMessage Put(Project project)
        {
            var currentProject = new Project();
            using (var db = new ResearchLinksContext())
            {
                db.Configuration.ProxyCreationEnabled = false;
                try
                {
                    // Verify that the user is the owner
                    currentProject = db.Projects.Where(p => p.ProjectId == project.ProjectId && p.UserName.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
                    if (currentProject == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("Project not found for user " + User.Identity.Name + ".")
                        };
                    }
                    currentProject.DateUpdated = DateTime.Now;
                    currentProject.Name = project.Name;
                    currentProject.Description = project.Description;
                    db.Entry(currentProject).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Error updating project: " + ex.Message),
                        ReasonPhrase = "Database Exception"
                    };
                }
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, currentProject);
            string uri = Url.Link("DefaultApi", new { id = project.ProjectId });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // DELETE api/projects/5
        public HttpResponseMessage Delete(int id)
        {
            using (var db = new ResearchLinksContext())
            {
                db.Configuration.ProxyCreationEnabled = false;
                try
                {
                    // Verify that the user is the owner
                    var currentProject = db.Projects.Where(p => p.ProjectId == id && p.UserName.ToLower() == User.Identity.Name.ToLower()).FirstOrDefault();
                    if (currentProject == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("Project not found for user " + User.Identity.Name + ".")
                        };
                    }
                    db.Entry(currentProject).State = EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Error deleting project: " + ex.Message),
                        ReasonPhrase = "Database Exception"
                    };
                }
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
