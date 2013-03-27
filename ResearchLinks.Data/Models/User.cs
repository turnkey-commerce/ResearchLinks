using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ResearchLinks.Data.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "User Name is required"), Display(Name = "User Name"), MinLength(3), MaxLength(100)]
        public string UserName { get; set; }

        [Required, MaxLength(64)]
        public byte[] PasswordHash { get; set; }

        [Required, MaxLength(128)]
        public byte[] PasswordSalt { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(200)]
        public string Comment { get; set; }

        [Display(Name = "Approved?")]
        public bool IsApproved { get; set; }

        [Display(Name = "Create Date")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Last Login Date")]
        public DateTime? DateLastLogin { get; set; }

        [Display(Name = "Last Activity Date")]
        public DateTime? DateLastActivity { get; set; }

        [Display(Name = "Last Password Change Date")]
        public DateTime DateLastPasswordChange { get; set; }

        [MaxLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(50), Display(Name = "Registration Code")]
        public string RegistrationCode { get; set; }

        [Display(Name = "Registration Code Expiration")]
        public DateTime? RegistrationCodeExpiration { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
   
        public bool Equals(User other)
        {
            if (this.UserName == other.UserName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}
