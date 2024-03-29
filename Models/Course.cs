﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingApplication.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Course Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
