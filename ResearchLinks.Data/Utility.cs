using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearchLinks.Data.Models;

namespace ResearchLinks.Data
{
    class Utility
    {
        public static void CreateNewUser(ResearchLinksContext context, string username, string password, string email, string role, string firstName, string lastName)
        {
            byte[] passwordSalt;
            byte[] passwordHash;
            GetPasswordHash(password, out passwordSalt, out passwordHash);

            User user = new User
            {
                UserName = username,
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsApproved = true,
                DateCreated = DateTime.Now,
                DateLastPasswordChange = DateTime.Now,
                FirstName = firstName,
                LastName = lastName
            };
            User existingUser = context.Users.Find(username);
            Role adminRole = context.Roles.Find(role);

            if (existingUser == null)
            {
                context.Users.Add(user);
                existingUser = user;
                user.Roles = new List<Role>();
                user.Roles.Add(adminRole);
            }
            else
            {
                if (!existingUser.Roles.Contains(adminRole))
                {
                    existingUser.Roles = new List<Role>();
                    existingUser.Roles.Add(adminRole);
                }
            }
            context.SaveChanges();
        }

        public static void GetPasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
