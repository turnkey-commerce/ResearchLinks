using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearchLinks.Data.Models;

namespace ResearchLinks.Data.Repository
{
    public interface IResearchItemRepository : IRepository<ResearchItem>
    {
        IEnumerable<ResearchItem> GetByUserAndProject(string userName, int ProjectId);

        ResearchItem GetByUser(int researchItemId, string userName);
    }
}
