using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ResearchLinks.Data.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }

        [Required, MaxLength(255)]
        public string Url { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        [Required]
        public int ResearchItemId { get; set; }
        public ResearchItem ResearchItem { get; set; }
    }
}
