using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingApplication.Models
{
    public class Topic
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Teacher { get; set; }
        public string Level { get; set; }
        public enum ELevel { NA = 0, Beginner = 1, Intermediate = 2, Advance = 3 }
        public string Image { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Course")]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Duration should be greater than {1} hour")]
        public int Duration { get; set; }

    }
}
