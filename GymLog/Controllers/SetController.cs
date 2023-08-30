using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Controllers
{
	public class SetController : Controller
	{
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SetController(AppDbContext context, UserManager<AppUser> userManager)
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
                    var sets = _context.Sets.ToList();
                    return View(sets);
                }
            }
            return RedirectToAction("Login", "Account");
		}

		public async Task<IActionResult> Create()
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
                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
		}

		[HttpPost]
		public IActionResult Create(Set set)
		{
			if(!ModelState.IsValid)
			{
				return View(set);
			}
			_context.Sets.Add(set);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int id)
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
                    var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == id);
                    if (set == null) return View("Error");
                    var setVM = new SetVM()
                    {
                        Id = id,
                        Weight = set.Weight,
                        Reps = set.Reps,
                        Description = set.Description,
                        WorkoutSegmentId = set.WorkoutSegmentId,
                    };
                    return View(setVM);
                }
            }
            return RedirectToAction("Login", "Account");

		}
		[HttpPost]
		public async Task<IActionResult> EditPost(SetVM setVM)
		{
			if(!ModelState.IsValid) 
			{
				return View(setVM);
			}
            var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == setVM.Id);

            if (set != null)
            {
                set.Weight = setVM.Weight;
                set.Reps = setVM.Reps;
                set.Description = setVM.Description;
                set.WorkoutSegmentId = setVM.WorkoutSegmentId;
                _context.Update(set);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
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
                    var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == id);
                    if (set == null) return View("Error");
                    return View(set);
                }
            }
            return RedirectToAction("Login", "Account");

		}
		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteSet(Set Set)
		{
			_context.Remove(Set);
			_context.SaveChanges();
			return RedirectToAction("Index");		}
	}
}
