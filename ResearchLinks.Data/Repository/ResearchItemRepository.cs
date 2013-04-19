using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearchLinks.Data.Models;

namespace ResearchLinks.Data.Repository
{
    public class ResearchItemRepository : Repository<ResearchItem>, IResearchItemRepository
    {
        public ResearchItemRepository() : base() 
        {
            dataContext.Configuration.ProxyCreationEnabled = false;
        }

        public IEnumerable<Models.ResearchItem> GetByUserAndProject(string userName, int projectId)
        {
            var researchItems = dataContext.ResearchItems
                .Where(u => u.UserName.ToLower() == userName.ToLower() && u.ProjectId == projectId);
            return researchItems;
        }

        public Models.ResearchItem GetByUser(int researchItemId, string userName)
        {
            var researchItem = dataContext.ResearchItems.Where(r => r.ResearchItemId == researchItemId && r.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
            return researchItem;
        }

    }
}
