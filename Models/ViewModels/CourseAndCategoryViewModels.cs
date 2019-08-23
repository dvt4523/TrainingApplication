using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingApplication.Models.ViewModels
{
    public class CourseAndCategoryViewModels
    {
        public IEnumerable<Category> CategoryList { get; set; }
        public Course Course { get; set; }
        public List<string> CourseList { get; set; }
        public string StatusMessage { get; set; }
    }
}
