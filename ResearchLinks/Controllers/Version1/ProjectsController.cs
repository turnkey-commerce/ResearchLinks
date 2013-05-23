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

namespace ResearchLinks.Controllers.Version1
{
    [Authorize]
    public class ProjectsController : ProjectsControllerBase
    {
        // Constructor here to inject dependencies in concrete class.
        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // Nothing to override in Version1
    }
}
