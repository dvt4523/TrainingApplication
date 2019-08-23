using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingApplication.Models.ViewModels
{
    public class TopicViewModel
    {
        public Topic Topic { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<Course> Course { get; set; }
        public IEnumerable <Course> CourseList{ get; set; }
    }
}