using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ResearchLinks.Data.Models;
using ResearchLinks.Data;

namespace ResearchLinks.Controllers
{
    public class ProjectsController : ApiController
    {
        // GET api/projects
        public List<Project> Get()
        {
            var projects = new List<Project>();
            using (var db = new ResearchLinksContext())
            {
                int userId = 1;
                //Project project = new Project{Name = "Test 2", DateCreated = DateTime.Now, DateUpdated = DateTime.Now, Description = "Test Project", UserId = 1};
                //db.Projects.Add(project);
                //db.SaveChanges();
                db.Configuration.ProxyCreationEnabled = false;

                projects = db.Projects.Where(p => p.UserId == userId).ToList();
            }
            return projects;
        }

        // GET api/projects/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/projects
        public void Post([FromBody]string value)
        {
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
