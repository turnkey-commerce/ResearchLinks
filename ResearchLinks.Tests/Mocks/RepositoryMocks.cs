using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearchLinks.Data.Models;
using ResearchLinks.Data.Repository;
using Moq;

namespace ResearchLinks.Tests.Mocks
{
    public class RepositoryMocks
    {
        private List<Project> projects = new List<Project>();

        public RepositoryMocks() {
            // projects
            projects.Add(new Project() { Name = "Test Project 1", UserName = "james", ProjectId = 1 });
            projects.Add(new Project() { Name = "Test Project 2", UserName = "james", ProjectId = 2 });
            projects.Add(new Project() { Name = "Test Project 3", UserName = "john", ProjectId = 3 });
        
        }

        public Mock<IProjectRepository> GetProjectsRepository(bool throwsDbError)
        {
            var mockProjectRepository = new Mock<IProjectRepository>();

            var projectListJames = new List<Project>();
            projectListJames.Add(projects[0]);
            projectListJames.Add(projects[1]);

            var projectListJohn = new List<Project>();
            projectListJohn.Add(projects[2]);

            if (throwsDbError) {
                mockProjectRepository.Setup(m => m.GetByUser(It.IsAny<string>())).Throws(new ApplicationException("Error retrieving projects"));
            } else {
                mockProjectRepository.Setup(m => m.GetByUser("james")).Returns(projectListJames.AsQueryable());
                mockProjectRepository.Setup(m => m.GetByUser("john")).Returns(projectListJohn.AsQueryable());
            }

            return mockProjectRepository;
        }
    }
}
