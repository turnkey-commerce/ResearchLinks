using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ResearchLinks.Data.Models
{
    public class Role
    {
        [Key, Display(Name = "Role Name"), Required(ErrorMessage = "Role Name"), MaxLength(100), MinLength(3)]
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
