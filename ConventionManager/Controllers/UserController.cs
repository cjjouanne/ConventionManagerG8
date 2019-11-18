using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConventionManager.Data;
using ConventionManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;


namespace ConventionManager.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private readonly IUpload _uploadService;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUpload uploadService, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _uploadService = uploadService;
            _configuration = configuration;
        }

        public async Task<IActionResult> ShowProfile(string userName)
        {
            ApplicationUser user;
            if (userName == "Me")
            {
                user = await _userManager.GetUserAsync(HttpContext.User);
            }
            else
            {
                user = await _userManager.FindByNameAsync(userName);
            }

            var thisUserName = _userManager.GetUserName(HttpContext.User);

            ViewData["thisUserName"] = thisUserName;

            return View(user);
        }

        [HttpGet]
        [Authorize(Roles = "Organizer")]
        public IActionResult ChangeRole()
        {
            var users = _context.Users.Where(u => u.Id != _userManager.GetUserId(HttpContext.User)).ToList();
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Id", "Name");
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles ="Organizer")]
        public async Task<IActionResult> ChangeUserRole(string userId, string roleId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var newRole = _roleManager.Roles.SingleOrDefault(r => r.Id == roleId);

                if (user == null)
                {
                    return NotFound();
                }
                await _userManager.AddToRoleAsync(user, newRole.Name);
                    
                
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "EventCenter");
            }
            return View();
        }
    }
}
