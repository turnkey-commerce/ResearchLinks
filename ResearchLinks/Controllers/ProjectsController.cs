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

namespace ResearchLinks.Controllers
{
    [Authorize]
    public class ProjectsController : ApiController
    {
        // GET api/projects
        public List<Project> Get()
        {
            var projects = new List<Project>();
            using (var db = new ResearchLinksContext())
            {
                string userName = User.Identity.Name;
                db.Configuration.ProxyCreationEnabled = false;

                projects = db.Projects.Where(p => p.UserName == userName).ToList();
            }
            return projects;
        }

        // GET api/projects/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/projects
        public HttpResponseMessage Post(Project project)
        {
            if (ModelState.IsValid)
            {

                using (var db = new ResearchLinksContext())
                {
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
                            { Content = new StringContent("Error saving project: " + ex.Message),
                                ReasonPhrase = "Database Exception"
                            };
                    }
                }
            }
            var response = Request.CreateResponse(HttpStatusCode.Created, project);
            return response;
        }

        // PUT api/projects/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/projects/5
        public void Delete(int id)
        {
        }
    }
}
