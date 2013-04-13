using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearchLinks.Data.Models;

namespace ResearchLinks.Data.Repository
{
    public interface IProjectRepository : IRepository<Project>
    {
        IEnumerable<Project> GetByUser(string userName);

        Project GetByUser(int projectId, string userName);
    }
}
