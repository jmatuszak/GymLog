using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Controllers
{
    public class BodyPartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BodyPartController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return RedirectToAction("Login", "Account");

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                if (role == "user")
                {
                    RedirectToAction("Index", "Home");
                }
                else if (role == "admin")
                {
                    var bodyParts = _context.BodyParts.ToList();
                    return View(bodyParts);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BodyPart bodyPart)
        {
            if(!ModelState.IsValid)
            {
                return View(bodyPart);
            }
            
            _context.BodyParts.Add(bodyPart);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var bodyPart = await _context.BodyParts.FirstOrDefaultAsync(a=>a.Id == id);
            if (bodyPart == null) return View("Error");
            var bodyPartVM = new BodyPartVM
            {
                Name = bodyPart.Name,
            };
            return View(bodyPartVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,BodyPartVM bodyPartVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit BodyPart");
                return View("Edit", bodyPartVM);
            }
            var bodyPart = await _context.BodyParts.FirstOrDefaultAsync(a=>a.Id == id);

            if (bodyPart != null)
            {
                bodyPart.Id = id;
                bodyPart.Name = bodyPartVM.Name;
                _context.Update(bodyPart);
                _context.SaveChanges();
			}
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var bodyPart = await _context.BodyParts.FirstOrDefaultAsync(a=>a.Id == id);
            if (bodyPart == null) return View("Error");
            return View(bodyPart);
        }
		[HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteBodyPartPart(int id)
        {
            var bodyPart = await _context.BodyParts.FirstOrDefaultAsync(a=>a.Id == id);
            if (bodyPart == null) return View("Error");
            _context.Remove(bodyPart);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


	}
}
