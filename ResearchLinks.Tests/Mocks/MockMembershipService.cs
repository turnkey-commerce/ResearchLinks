using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearchLinks.Services;
using System.Web.Security;
using NUnit.Framework;

namespace ResearchLinks.Tests.Mocks
{
    public class MockMembershipService : IMembershipService
    {
        public int MinPasswordLength
        {
            get { return 10; }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (userName == "james" && password == "goodPassword") {
                return true;
            } else if (userName == "john" && password == "goodPassword") {
                return true;
            }
            return false;
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (userName == "duplicate@email.com") {
                return MembershipCreateStatus.DuplicateUserName;
            }

            // verify that values are what we expected
            Assert.AreEqual("goodPassword", password);
            Assert.AreEqual("user@info.com", email);

            return MembershipCreateStatus.Success;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return (userName == "user@info.com" && oldPassword == "goodOldPassword" && newPassword == "goodNewPassword");
        }
    }
}
