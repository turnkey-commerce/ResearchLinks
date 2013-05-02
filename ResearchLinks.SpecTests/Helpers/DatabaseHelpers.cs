using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using ResearchLinks.Data;

namespace ResearchLinks.SpecTests.Helpers
{
    [Binding]
    public class DatabaseHelpers
    {
        [BeforeScenario]
        public void CleanDatabase()
        {
            var context = new ResearchLinksContext();
            foreach (var project in context.Projects.ToList())
            {
                context.Projects.Remove(project);
            }
            context.SaveChanges();

        }
    }
}
