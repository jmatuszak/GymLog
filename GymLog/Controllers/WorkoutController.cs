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
	public class WorkoutController : BaseController
	{
		private readonly AppDbContext _context;
		private readonly UserManager<AppUser> _userManager;

		public WorkoutController(AppDbContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		public async Task<IActionResult> Index()
		{
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return RedirectToAction("Login", "Account");
            var userWorkouts = _context.Workouts.Include(t => t.Template).Where(x=>x.AppUserId.Equals(user.Id)).ToList();
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

        //<-----------------------   Set   ---------------------> 

        public IActionResult AddSet(WorkoutVM workoutVM, [FromQuery(Name = "segment")] int segment)
        {
            if (workoutVM == null) throw new ArgumentNullException();
            if (workoutVM.WorkoutSegmentsVM == null) throw new ArgumentNullException();
            if (workoutVM.WorkoutSegmentsVM[segment] == null) throw new ArgumentNullException();
            workoutVM.WorkoutSegmentsVM[segment].SetsVM ??= new List<SetVM>();
            workoutVM.WorkoutSegmentsVM[segment].SetsVM.Add(new SetVM());
            return View(workoutVM.ActionName, workoutVM);
        }
        public IActionResult RemoveSet(WorkoutVM workoutVM, [FromQuery(Name = "segment")] int segment)
        {
            if (workoutVM == null) throw new ArgumentNullException();
            if (workoutVM.WorkoutSegmentsVM == null) throw new ArgumentNullException();
            if (workoutVM.WorkoutSegmentsVM[segment].SetsVM == null) return View("Error");
            workoutVM.WorkoutSegmentsVM[segment].SetsVM.RemoveAt(workoutVM.WorkoutSegmentsVM[segment].SetsVM.Count - 1);
            ModelState.Clear();
            return View(workoutVM.ActionName, workoutVM);
        }



        //<-----------------------   Excercise   ---------------------> 

        public IActionResult AddWorkoutSegment(WorkoutVM workoutVM)
        {
            workoutVM.WorkoutSegmentsVM ??= new List<WorkoutSegmentVM>();
            var segment = new WorkoutSegmentVM()
            {
                SetsVM = new List<SetVM>() { new SetVM() }
            };
            workoutVM.WorkoutSegmentsVM.Add(segment);
            return View(workoutVM.ActionName, workoutVM);
        }
        public IActionResult RemoveWorkoutSegment(WorkoutVM workoutVM, [FromQuery(Name = "segment")] int segment)
        {
            if (workoutVM.WorkoutSegmentsVM == null) return View("Error");
            workoutVM.WorkoutSegmentsVM.RemoveAt(segment);
            ModelState.Clear();
            return View(workoutVM.ActionName, workoutVM);
        }


		//<-----------------------   Workout   ---------------------> 
		public async Task<IActionResult> Execute(int id)
		{
            var template = await Context.Templates.Include(t => t.WorkoutSegments).FirstOrDefaultAsync(t => t.Id == id);
            var templateVM = new TemplateVM() { ActionName = "Create" };
            if (template == null) return View("Error");

            templateVM = TemplateToTemplateVM(template,templateVM);

			var workoutVM = new WorkoutVM()
            {
                Name = templateVM.Name,
                WorkoutSegmentsVM= templateVM.WorkoutSegmentsVM,
                Excercises = templateVM.Excercises,
                TemplateId = id,
                ActionName = templateVM.ActionName,
                StartDate = DateTime.Now,
            };
			return View(workoutVM.ActionName, workoutVM);
		}

		//<-----------------------   Create   ---------------------> 
		public async Task<IActionResult> Create(WorkoutVM? workoutVM)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
			workoutVM ??= new WorkoutVM()
            {
                StartDate = DateTime.Now,
            };
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            workoutVM.Excercises = _context.Excercises.ToList();
            workoutVM.WorkoutSegmentsVM ??= new List<WorkoutSegmentVM>()
                {
                    new WorkoutSegmentVM()
                    {
                        SetsVM = new List<SetVM>{ new SetVM() { isDone = false } }
                    }
                };
            workoutVM.ActionName = "create";
            return View(workoutVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(WorkoutVM workoutVM)
        {
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var user = await _userManager.GetUserAsync(HttpContext.User);
            Workout workout = new Workout()
            {
                Name = workoutVM.Name,
                AppUserId = user.Id,
                StartDate = workoutVM.StartDate,
                EndDate = DateTime.Now,
                TemplateId = workoutVM.TemplateId,
				WorkoutSegments = new List<WorkoutSegment>(),
			};

            if (workoutVM.WorkoutSegmentsVM != null)
                foreach (var segment in workoutVM.WorkoutSegmentsVM)
                {
                    List<Set> sets = new List<Set>();
                    if (segment.SetsVM != null)
                        foreach (var set in segment.SetsVM)
                            sets.Add(new Set()
                            {
                                Weight = set.Weight,
                                Reps = set.Reps,
                                //WorkoutSegmentId = template.Id
                            });
                    workout.WorkoutSegments.Add(new WorkoutSegment
                    {
                        WeightType = segment.WeightType,
                        Description = segment.Description,
                        //Order = segment.Order,
                        Sets = sets,
                        ExcerciseId = segment.ExcerciseId,
                    });
                }
            _context.Add(workout);
            _context.SaveChanges();

            foreach (var segment in workout.WorkoutSegments)
            {
                foreach (var set in segment.Sets)
                    set.WorkoutSegmentId = segment.Id;
            }
            _context.Update(workout);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }

}
