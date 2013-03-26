using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ResearchLinks.Data.Models
{

    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public virtual List<ResearchItem> ResearchItems { get; set; }

    }
}
