using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResearchLinks.Data.Models;

namespace ResearchLinks.DTO
{
    public class ResearchItemDto
    {
        public List<ResearchItem> ResearchItems { get; set; }
        public ResearchItemMeta Meta { get; set; }
    }

    public class ResearchItemMeta
    {
        public string ProjectName { get; set; }
        public int NumberResearchItems { get; set; }
    }
}