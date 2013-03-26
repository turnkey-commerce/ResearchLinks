using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ResearchLinks.Data.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        [Required, MaxLength(5000)]
        public string Text { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        [Required]
        public int ResearchItemId { get; set; }
        
        public ResearchItem ResearchItem { get; set; }
    }
}
