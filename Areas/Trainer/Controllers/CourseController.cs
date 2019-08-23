using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainingApplication.Data;
using TrainingApplication.Models;
using TrainingApplication.Models.ViewModels;
using TrainingApplication.Ultility;

namespace TrainingApplication.Areas.Trainer.Controllers
{
    [Authorize(Roles = SD.TrainerUser)]
    [Area("Trainer")]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CourseController(ApplicationDbContext db)
        {
            _db = db;
        }
        [TempData]
        public string StatusMessage { get; set; }
        //Get
        public async Task<IActionResult> Index()
        {
            return View(await _db.Course.Include(s=>s.Category).ToListAsync());
        }
        
        //Get - Add
        public async Task<IActionResult> Add()
        {
            CourseAndCategoryViewModels model = new CourseAndCategoryViewModels()
            {
                CategoryList = await _db.Category.ToListAsync(),
                Course = new Models.Course(),
                CourseList = await _db.Course.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
            };
            return View(model);
        }
        //Post - Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CourseAndCategoryViewModels model)
        {
            if (ModelState.IsValid)
            {
                var doesCourseExists = _db.Course.Include(s => s.Category).Where(s => s.Name == model.Course.Name && s.Category.Id == model.Course.CategoryId);
                if (doesCourseExists.Count() > 0)
                {
                    StatusMessage = "Error : Course already exists in " + doesCourseExists.First().Category.Name + " category. Please use another name.";
                }
                else
                {
                    _db.Course.Add(model.Course);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            CourseAndCategoryViewModels modelVM = new CourseAndCategoryViewModels()
            {
                CategoryList = await _db.Category.ToListAsync(),
                Course = model.Course,
                CourseList = await _db.Course.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage=StatusMessage,
            };
            return View(modelVM);
        }

        [ActionName("GetCourse")]
        public async Task<IActionResult> GetCourse(int id)
        {
            List<Course> courses = new List<Course>();

            courses = await (from course in _db.Course
                             where course.CategoryId == id
                             select course).ToListAsync();
            return Json(new SelectList(courses, "Id", "Name"));
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<Course> subCategories = new List<Course>();

            subCategories = await (from subCategory in _db.Course
                                   where subCategory.CategoryId == id
                                   select subCategory).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }

        //Get - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _db.Course.SingleOrDefaultAsync(m => m.Id == id);

            if (subCategory == null)
            {
                return NotFound();
            }

            CourseAndCategoryViewModels model = new CourseAndCategoryViewModels()
            {
                CategoryList = await _db.Category.ToListAsync(),
                Course = subCategory,
                CourseList = await _db.Course.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }
        //Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseAndCategoryViewModels model)
        {
            if (ModelState.IsValid)
            {
                var doesCourseExists = _db.Course.Include(s => s.Category).Where(s => s.Name == model.Course.Name && s.Category.Id == model.Course.CategoryId);
                if (doesCourseExists.Count() > 0)
                {
                    StatusMessage = "Error : Course already exists in " + doesCourseExists.First().Category.Name + " category. Please use another name.";
                }
                else
                {
                    var courseFromDb = await _db.Course.FindAsync(id);
                    courseFromDb.Name = model.Course.Name;
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            CourseAndCategoryViewModels modelVM = new CourseAndCategoryViewModels()
            {
                CategoryList = await _db.Category.ToListAsync(),
                Course = model.Course,
                CourseList = await _db.Course.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage,
            };
            modelVM.Course.Id = id;
            return View(modelVM);
        }
        //GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await _db.Course.Include(s => s.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }
        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _db.Course.SingleOrDefaultAsync(m => m.Id == id);
            _db.Course.Remove(subCategory);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}