using GymLog.Data;
using GymLog.Interfaces;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Controllers
{
    public class BodyPartController : Controller
    {
        private readonly IBodyPartRepository _bodyPartRepository;
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



        public async Task<IActionResult> Detail(int id)
        {
            var bodyPart = await _bodyPartRepository.GetById(id);
            if (bodyPart == null)
            {
                return NotFound();
            }
            return View(bodyPart);
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
            
            _bodyPartRepository.Add(bodyPart);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var bodyPart = await _bodyPartRepository.GetByIdAsNoTracking(id);
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
                return View("Edtit", bodyPartVM);
            }
            var bodyPart = await _bodyPartRepository.GetByIdAsNoTracking(id);

            if (bodyPart != null)
            {
                bodyPart.Id = id;
                bodyPart.Name = bodyPartVM.Name;
				_bodyPartRepository.Update(bodyPart);
			}

            
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var bodyPart = await _bodyPartRepository.GetByIdAsNoTracking(id);
            if (bodyPart == null) return View("Error");
            return View(bodyPart);
        }
		[HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteBodyPartPart(int id)
        {
            var bodyPart = await _bodyPartRepository.GetByIdAsNoTracking(id);
            if (bodyPart == null) return View("Error");
            _bodyPartRepository.Delete(bodyPart);
            return RedirectToAction("Index");
        }


	}
}
