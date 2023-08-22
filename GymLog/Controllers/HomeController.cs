using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using NuGet.Protocol;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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


        public async Task<ActionResult> MaxWeight(int id)
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

        public async Task<ActionResult> ExerciseProgress(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var workouts = await _context.Workouts.Include(x => x.WorkoutSegments)
                .ThenInclude(segment => segment.Sets)
                .Where(x => x.AppUserId == user.Id).ToListAsync();

            var labels = new List<DateTime>();
            var values = new List<float>();

            foreach (var workout in workouts)
            {
                if (workout.WorkoutSegments != null)
                    foreach (var segment in workout.WorkoutSegments)
                    {
                        float maxValue = 0;
                        if (segment.ExerciseId == id && segment.Sets != null)
                        {
                            foreach(var set in segment.Sets)
                            {
                                if (maxValue < set.Weight) maxValue = (float)set.Weight;
                            }
                            if (maxValue > 0)
                            {
                                labels.Add(workout.EndDate);
                                values.Add(maxValue);
                            }

                        }
                    }
            }
            var weightAndDateDic = new Dictionary<DateTime, float>();
            for(int i=0; i<values.Count; i++)
            {
                weightAndDateDic.Add(labels[i], values[i]);
            }
			var sortedDictionary = weightAndDateDic.OrderBy(pair => pair.Key).
                ToDictionary(pair => pair.Key, pair => pair.Value);

			var chartDataProgressVM = new ChartDataProgressVM();

			
            chartDataProgressVM.Labels = new DateTime[values.Count];
            chartDataProgressVM.Values = new float[values.Count];
            for(int i=0;i<values.Count;i++)
            {
                chartDataProgressVM.Labels[i] = sortedDictionary.Keys.ElementAt(i);
                chartDataProgressVM.Values[i] = sortedDictionary.Values.ElementAt(i);

			}
			return PartialView("_ExerciseProgressChart", chartDataProgressVM);
        }

	}
}
