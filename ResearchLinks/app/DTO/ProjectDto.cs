using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResearchLinks.Data.Models;

namespace ResearchLinks.app.DTO
{
    public class ProjectDto
    {
        public List<Project> Projects { get; set; }
        public ProjectMeta Meta { get; set; }
    }

    public class ProjectMeta
    {
        public int NumberProjects { get; set; }
    }
}