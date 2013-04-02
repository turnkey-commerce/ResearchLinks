using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ResearchLinks.Data.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required, MaxLength(50), MinLength(2)]
        public string Name { get; set; }

        [Required]
        public int ResearchItemId { get; set; }
        public virtual ResearchItem ResearchItem { get; set; }
    }
}
