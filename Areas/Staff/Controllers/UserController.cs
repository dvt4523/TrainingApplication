﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrainingApplication.Data;
using TrainingApplication.Ultility;

namespace TrainingApplication.Areas.Staff.Controllers
{
    [Authorize(Roles = SD.StaffUser)]
    [Area("Staff")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(await _db.ApplicationUser.Where(u=>u.Id != claim.Value).ToListAsync());
        }
    }
}
