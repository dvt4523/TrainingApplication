using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingApplication.Data;
using TrainingApplication.Models.ViewModels;
using TrainingApplication.Models;
using TrainingApplication.Ultility;

namespace TrainingApplication.Areas.Trainer.Controllers
{
    [Authorize(Roles = SD.TrainerUser)]
    [Area("Trainer")]
    public class TopicController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        [BindProperty]
        public TopicViewModel TopicVM { get; set; }

        public TopicController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            TopicVM = new TopicViewModel()
            {
                Category = _db.Category,
                Topic = new Models.Topic(),
                CourseList = _db.Course.ToList()
            };
        }
        //Index
        public async Task<IActionResult> Index()
        {
            var topics = await _db.Topic.Include(m => m.Category).Include(m => m.Course).ToListAsync();
            return View(topics);
        }
        //GET - Add
        public IActionResult Add()
        {
            return View(TopicVM);
        }

        //Post - Add
        [HttpPost, ActionName("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPOST()
        {
            //TopicVM.Topic.CourseId = Convert.ToInt32(Request.Form["CourseId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(TopicVM);
            }

            _db.Topic.Add(TopicVM.Topic);
            await _db.SaveChangesAsync();

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var TopicfromDB = await _db.Topic.FindAsync(TopicVM.Topic.Id);

            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, TopicVM.Topic.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                TopicfromDB.Image = @"\images\" + TopicVM.Topic.Id + extension;
            }
            else
            {
                //no file was uploaded, so use default
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultTopicImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + TopicVM.Topic.Id + ".png");
                TopicfromDB.Image = @"\images\" + TopicVM.Topic.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //Get - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TopicVM.Topic= await _db.Topic.Include(m => m.Category).Include(m => m.Course).SingleOrDefaultAsync(m => m.Id == id);
            TopicVM.Course = await _db.Course.Where(s => s.CategoryId == TopicVM.Topic.CategoryId).ToListAsync();

            if (TopicVM.Topic == null)
            {
                return NotFound();
            }
            return View(TopicVM);
        }
        //Post - Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                TopicVM.Course = await _db.Course.Where(s => s.CategoryId == TopicVM.Topic.CategoryId).ToListAsync();
                return View(TopicVM);
            }

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var menuItemFromDb = await _db.Topic.FindAsync(TopicVM.Topic.Id);

            if (files.Count > 0)
            {
                //New Image has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original file
                var imagePath = Path.Combine(webRootPath, menuItemFromDb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                //we will upload the new file
                using (var filesStream = new FileStream(Path.Combine(uploads, TopicVM.Topic.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                menuItemFromDb.Image = @"\images\" + TopicVM.Topic.Id + extension_new;
            }

            menuItemFromDb.Name = TopicVM.Topic.Name;
            menuItemFromDb.Description = TopicVM.Topic.Description;
            menuItemFromDb.Duration = TopicVM.Topic.Duration;
            menuItemFromDb.Level = TopicVM.Topic.Level;
            menuItemFromDb.CategoryId = TopicVM.Topic.CategoryId;
            menuItemFromDb.CourseId = TopicVM.Topic.CourseId;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Get - Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TopicVM.Topic = await _db.Topic.Include(m => m.Category).Include(m => m.Course).SingleOrDefaultAsync(m => m.Id == id);

            if (TopicVM.Topic == null)
            {
                return NotFound();
            }

            return View(TopicVM);
        }

        //Post - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Topic topic = await _db.Topic.FindAsync(id);

            if (topic != null)
            {
                var imagePath = Path.Combine(webRootPath, topic.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _db.Topic.Remove(topic);
                await _db.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }
    }
}