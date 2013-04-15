using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ResearchLinks.Services
{
    public class AccountRoleService : IRoleService
    {
        private RoleProvider _provider;

        public AccountRoleService()
            : this(null)
        {
        }

        public AccountRoleService(RoleProvider provider)
        {
            _provider = provider ?? Roles.Provider;
        }


        #region IRoleService Members

        public string[] GetRolesForUser(string userName)
        {
            return _provider.GetRolesForUser(userName);
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            return _provider.IsUserInRole(userName, roleName);
        }

        public void RemoveUserFromRole(string userName, string roleName)
        {
            string[] userNames = new string[] { userName };
            string[] roleNames = new string[] { roleName };
            _provider.RemoveUsersFromRoles(userNames, roleNames);
        }

        public void AddUserToRole(string userName, string roleName)
        {
            string[] userNames = new string[] { userName };
            string[] roleNames = new string[] { roleName };
            _provider.AddUsersToRoles(userNames, roleNames);
        }

        #endregion
    }

    public interface IRoleService
    {
        string[] GetRolesForUser(string userName);
        bool IsUserInRole(string userName, string roleName);
        void RemoveUserFromRole(string userName, string roleName);
        void AddUserToRole(string userName, string roleName);
    }
}