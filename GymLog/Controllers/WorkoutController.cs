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
	public class WorkoutController : Controller
	{
		private readonly AppDbContext _context;
		private readonly UserManager<AppUser> _userManager;

		public WorkoutController(AppDbContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		public IActionResult Index()
		{
			var list = _context.Workouts.Include(t => t.Template).ToList();
			return View(list);
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
        // TEMPLATE TO  VIEW MODEL
        public async Task<TemplateVM> TemplateToTemplateVM(int id)
        {
            var templateVM = new TemplateVM();
		    var template = await _context.Templates.Include(t => t.WorkoutSegments).FirstOrDefaultAsync(t => t.Id == id);
		    templateVM = templateVM ?? new TemplateVM();
		    templateVM.Excercises = _context.Excercises.ToList();
		    templateVM.WorkoutSegmentsVM = templateVM.WorkoutSegmentsVM ?? new List<WorkoutSegmentVM>();
		    templateVM.Id = template.Id;
		    templateVM.Name = template.Name;
		    templateVM.ActionName = "Create";

		    var segmentsVM = new List<WorkoutSegmentVM>();
		    if (template.WorkoutSegments != null && template.WorkoutSegments.Count > 0)
			    for (int t = 0; t < template.WorkoutSegments.Count; t++)
			    {
				    template.WorkoutSegments[t].Sets = _context.Sets.Where(s => s.WorkoutSegmentId == template.WorkoutSegments[t].Id).ToList();
				    var setsVM = new List<SetVM>();
				    var segmentVM = new WorkoutSegmentVM();
				    segmentVM.Id = template.WorkoutSegments[t].Id;
				    segmentVM.TemplateId = template.WorkoutSegments[t].TemplateId;
				    segmentVM.ExcerciseId = template.WorkoutSegments[t].ExcerciseId;
				    segmentVM.Description = template.WorkoutSegments[t].Description;
				    segmentVM.WeightType = template.WorkoutSegments[t].WeightType;
				    if (template.WorkoutSegments[t].Sets != null)
				    {
					    for (var s = 0; s < template.WorkoutSegments[t].Sets.Count; s++)
					    {
						    if (template.WorkoutSegments[t].Sets[s].Id != null)
						    {
							    setsVM.Add(new SetVM()
							    {
								    Id = template.WorkoutSegments[t].Sets[s].Id,
								    Description = template.WorkoutSegments[t].Sets[s].Description,
								    Weight = template.WorkoutSegments[t].Sets[s].Weight,
								    Reps = template.WorkoutSegments[t].Sets[s].Reps,
								    WorkoutSegmentId = template.WorkoutSegments[t].Sets[s].WorkoutSegmentId,
							    });
						    }
						    else
						    {
							    setsVM.Add(new SetVM()
							    {
								    Description = template.WorkoutSegments[t].Sets[s].Description,
								    Weight = template.WorkoutSegments[t].Sets[s].Weight,
								    Reps = template.WorkoutSegments[t].Sets[s].Reps,
								    WorkoutSegmentId = template.WorkoutSegments[t].Sets[s].WorkoutSegmentId,
							    });
						    }

					    }
				    }

				    segmentVM.SetsVM = setsVM;
				    templateVM.WorkoutSegmentsVM.Add(segmentVM);
			    }

			return templateVM;
		}

		//<-----------------------   Workout   ---------------------> 
		public async Task<IActionResult> Execute(int id)
		{
            var templateVM = new TemplateVM() { ActionName = "Create" };
            templateVM = await  TemplateToTemplateVM(id);

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
