using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ResearchLinks.Data.Models;
using ResearchLinks.Data.Repository;
using ResearchLinks.DTO;

namespace ResearchLinks.Controllers.Version1
{
    [Authorize]
    public class ProjectResearchItemsController : ProjectResearchItemsControllerBase
    {
        // Constructor here to inject dependencies in concrete class.
        public ProjectResearchItemsController(IResearchItemRepository researchItemRepository, IProjectRepository projectRepository)
        {
            _researchItemRepository = researchItemRepository;
            _projectRepository = projectRepository;
        }

        // Nothing to override in Version1

    }
}
