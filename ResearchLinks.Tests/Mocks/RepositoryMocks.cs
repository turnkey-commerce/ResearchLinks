using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearchLinks.Data.Models;
using ResearchLinks.Data.Repository;
using Moq;

namespace ResearchLinks.Tests.Mocks
{
    public enum ReturnType { Exception, Normal};
    public class RepositoryMocks
    {
        private List<Project> projects = new List<Project>();
        private List<ResearchItem> researchItems = new List<ResearchItem>();

        public RepositoryMocks() {
            // projects
            projects.Add(new Project() { Name = "Test Project 1", UserName = "james", ProjectId = 1 });
            projects.Add(new Project() { Name = "Test Project 2", UserName = "james", ProjectId = 2 });
            projects.Add(new Project() { Name = "Test Project 3", UserName = "john", ProjectId = 3 });

            //researchItems
            researchItems.Add(new ResearchItem() { Subject = "Test Research Item 1", UserName = "james", ProjectId = 1 });
            researchItems.Add(new ResearchItem() { Subject = "Test Research Item 2", UserName = "james", ProjectId = 1 });
            researchItems.Add(new ResearchItem() { Subject = "Test Research Item 3", UserName = "john", ProjectId = 3 });
        
        }

        public Mock<IProjectRepository> GetProjectsRepository(ReturnType returnType)
        {
            var mockProjectRepository = new Mock<IProjectRepository>();

            var projectListJames = new List<Project>();
            projectListJames.Add(projects[0]);
            projectListJames.Add(projects[1]);

            var projectListJohn = new List<Project>();
            projectListJohn.Add(projects[2]);

            if (returnType == ReturnType.Exception) {
                mockProjectRepository.Setup(m => m.GetByUser(It.IsAny<string>())).Throws(new ApplicationException("Database exception!"));
                mockProjectRepository.Setup(m => m.GetByUser(It.IsAny<int>(), It.IsAny<string>())).Throws(new ApplicationException("Database exception!"));
                mockProjectRepository.Setup(m => m.Insert(It.IsAny<Project>())).Throws(new ApplicationException("Database exception!"));
                mockProjectRepository.Setup(m => m.Delete(It.IsAny<Project>())).Throws(new ApplicationException("Database exception!"));
                mockProjectRepository.Setup(m => m.Commit()).Throws(new ApplicationException("Database exception!"));
            } else {
                mockProjectRepository.Setup(m => m.GetByUser("james")).Returns(projectListJames.AsQueryable());
                mockProjectRepository.Setup(m => m.GetByUser("john")).Returns(projectListJohn.AsQueryable());
                mockProjectRepository.Setup(m => m.GetByUser(1,"james")).Returns(projects[0]);
                mockProjectRepository.Setup(m => m.GetByUser(1, "john")).Returns((Project)null);
                mockProjectRepository.Setup(m => m.GetByUser(3, "john")).Returns(projects[2]);
                mockProjectRepository.Setup(m => m.Delete(It.IsAny<Project>()));
                mockProjectRepository.Setup(m => m.Insert(It.IsAny<Project>()));
                mockProjectRepository.Setup(m => m.Commit());
            }

            return mockProjectRepository;
        }

        public Mock<IResearchItemRepository> GetResearchItemsRepository(ReturnType returnType)
        {
            var mockResearchItemRepository = new Mock<IResearchItemRepository>();

            var researchItemListJames = new List<ResearchItem>();
            researchItemListJames.Add(researchItems[0]);
            researchItemListJames.Add(researchItems[1]);

            var researchItemListJohn = new List<ResearchItem>();
            researchItemListJohn.Add(researchItems[2]);

            if (returnType == ReturnType.Exception)
            {
                mockResearchItemRepository.Setup(m => m.GetByUserAndProject(It.IsAny<string>(), It.IsAny<int>())).Throws(new ApplicationException("Database exception!"));
                mockResearchItemRepository.Setup(m => m.GetByUser(It.IsAny<int>(), It.IsAny<string>())).Throws(new ApplicationException("Database exception!"));
                mockResearchItemRepository.Setup(m => m.Insert(It.IsAny<ResearchItem>())).Throws(new ApplicationException("Database exception!"));
                mockResearchItemRepository.Setup(m => m.Delete(It.IsAny<ResearchItem>())).Throws(new ApplicationException("Database exception!"));
                mockResearchItemRepository.Setup(m => m.Commit()).Throws(new ApplicationException("Database exception!"));
            }
            else
            {
                mockResearchItemRepository.Setup(m => m.GetByUserAndProject("james",1)).Returns(researchItemListJames.AsQueryable());
                mockResearchItemRepository.Setup(m => m.GetByUserAndProject("john", 3)).Returns(researchItemListJohn.AsQueryable());
                mockResearchItemRepository.Setup(m => m.GetByUser(1, "james")).Returns(researchItems[0]);
                mockResearchItemRepository.Setup(m => m.GetByUser(1, "john")).Returns((ResearchItem)null);
                mockResearchItemRepository.Setup(m => m.Delete(It.IsAny<ResearchItem>()));
                mockResearchItemRepository.Setup(m => m.Insert(It.IsAny<ResearchItem>()));
                mockResearchItemRepository.Setup(m => m.Commit());
            }

            return mockResearchItemRepository;
        }
    }
}
