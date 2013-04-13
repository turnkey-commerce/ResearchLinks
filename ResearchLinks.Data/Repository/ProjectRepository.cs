using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearchLinks.Data.Models;
using ResearchLinks.Data.Migrations;

namespace ResearchLinks.Data.Repository
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository() : base() 
        {
            dataContext.Configuration.ProxyCreationEnabled = false;
        }

        public IEnumerable<Project> GetByUser(string userName)
        {
            var projects = dataContext.Projects
                .Where(u => u.UserName.ToLower() == userName.ToLower());
            return projects;
        }

        public Project GetByUser(int projectId, string userName)
        {
            var project = dataContext.Projects.Where(p => p.ProjectId == projectId && p.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
            return project;
        }
    }
}
