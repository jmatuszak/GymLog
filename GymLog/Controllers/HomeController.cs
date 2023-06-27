using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;


namespace GymLog.Controllers
{
    public class HomeController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //Calendar
        public IActionResult Calendar()
        {
            var workouts = _context.Workouts.ToList();

            return View(workouts);
        }



        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return RedirectToAction("Login", "Account");
            var userWorkouts = _context.Workouts.Include(t => t.Template).Where(x => x.AppUserId.Equals(user.Id)).ToList();
            var sampleTemplates = _context.Templates.Where(x => x.AppUserId.Equals(null)).ToList();
            var userTemplates = _context.Templates.Where(x => x.AppUserId.Equals(user.Id)).ToList();

            var workoutsAndTemplatesVM = new WorkoutsAndTemplatesVM()
            {
                UserTemplatesVM = new List<TemplateVM>(),
                UserWorkoutsVM = new List<WorkoutVM>(),
                SampleTemplatesVM = new List<TemplateVM>(),
            };
            foreach (var item in sampleTemplates)
            {
                var itemVM = TemplateToTemplateVM(item, new TemplateVM());
                workoutsAndTemplatesVM.SampleTemplatesVM.Add(itemVM);
            }
            foreach (var item in userTemplates)
            {
                var itemVM = TemplateToTemplateVM(item, new TemplateVM());
                workoutsAndTemplatesVM.UserTemplatesVM.Add(itemVM);
            }
            foreach (var item in userWorkouts)
            {
                var itemVM = WorkoutToWorkoutVM(item, new WorkoutVM());
                workoutsAndTemplatesVM.UserWorkoutsVM.Add(itemVM);
            }
            return View(workoutsAndTemplatesVM);
        }
        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return RedirectToAction("Login", "Account");
            var userWorkouts = _context.Workouts.Include(t => t.Template).Where(x => x.AppUserId.Equals(user.Id)).ToList();
            var sampleTemplates = _context.Templates.Where(x => x.AppUserId.Equals(null)).ToList();
            var userTemplates = _context.Templates.Where(x => x.AppUserId.Equals(user.Id)).ToList();

            var workoutsAndTemplatesVM = new WorkoutsAndTemplatesVM()
            {
                UserTemplatesVM = new List<TemplateVM>(),
                UserWorkoutsVM = new List<WorkoutVM>(),
                SampleTemplatesVM = new List<TemplateVM>(),
            };
            foreach (var item in sampleTemplates)
            {
                var itemVM = TemplateToTemplateVM(item, new TemplateVM());
                workoutsAndTemplatesVM.SampleTemplatesVM.Add(itemVM);
            }
            foreach (var item in userTemplates)
            {
                var itemVM = TemplateToTemplateVM(item, new TemplateVM());
                workoutsAndTemplatesVM.UserTemplatesVM.Add(itemVM);
            }
            foreach (var item in userWorkouts)
            {
                var itemVM = WorkoutToWorkoutVM(item, new WorkoutVM());
                workoutsAndTemplatesVM.UserWorkoutsVM.Add(itemVM);
            }
            return View(workoutsAndTemplatesVM);
        }


        public async Task<ActionResult> PartialStatistic(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var workouts = await _context.Workouts.Include(x => x.WorkoutSegments)
                .ThenInclude(segment => segment.Sets)
                .Where(x => x.AppUserId == user.Id).ToListAsync();
            var exercise = await _context.Exercises.FirstOrDefaultAsync(x => x.Id == id);
            var maxWeight = workouts
                .Where(a => a.WorkoutSegments.Any(b => b.ExerciseId == id))
                .SelectMany(a => a.WorkoutSegments.Where(b => b.ExerciseId == id))
                .SelectMany(b => b.Sets)
                .OrderByDescending(c => c.Weight)
                .FirstOrDefault();


            /*            var exercise = await _context.Exercises.FirstOrDefaultAsync(a=>a.Id== id);
						if (exercise == null || maxWeight==null) return View("Error");
			*/
            var maxWeightVM = new MaxWeightVM()
            {
                Name = exercise.Name,
                Weight = (float)maxWeight.Weight
            };
            return PartialView("_MaxWeight", maxWeightVM);
        }

		public IActionResult WorkoutDetails(int id)
		{
			return ViewComponent("WorkoutDetails", id);
		}

	}
}
