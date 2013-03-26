using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ResearchLinks.Data.Models
{
    public class Link
    {
        [Key]
        public int LinkId { get; set; }

        [Required, MaxLength(100), MinLength(5)]
        public string Title { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [Required, MaxLength(255)]
        public string Url { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        [Required]
        public int ResearchItemId { get; set; }
        public virtual ResearchItem ResearchItem { get; set; }
    }
}
