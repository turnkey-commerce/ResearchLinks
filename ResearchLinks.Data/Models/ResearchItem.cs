using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ResearchLinks.Data.Models
{
    public class ResearchItem
    {
        [Key]
        public int ResearchItemId { get; set; }

        [Required, MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }

        [Required]
        public string UserName { get; set; }
        public User User { get; set; }

        public virtual List<Link> Links { get; set; }
        public virtual List<Note> Notes { get; set; }
        public virtual List<Image> Images { get; set; }
    }
}
