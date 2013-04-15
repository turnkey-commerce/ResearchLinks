using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearchLinks.Services;

namespace ResearchLinks.Tests.Mocks
{
    public class MockRoleService : IRoleService
    {
        public string[] GetRolesForUser(string userName)
        {
            var roles = new string[0];
            return roles;
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromRole(string userName, string roleName)
        {
            throw new NotImplementedException();
        }

        public void AddUserToRole(string userName, string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
