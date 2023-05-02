using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;


namespace GymLog.Controllers
{
	public class WorkoutController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public WorkoutController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			//_httpContextAccessor = httpContextAccessor;
		}
		public IActionResult Index()
		{
			var list = _context.WorkoutSegments.Include(s => s.Sets).Include(e => e.Excercise).Include(t => t.Template).ToList();
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
        public IActionResult RemoveSet(WorkoutVM workoutVM, [FromQuery(Name = "segment")] int segment, [FromQuery(Name = "set")] int set)
        {
            if (workoutVM == null) throw new ArgumentNullException();
            if (workoutVM.WorkoutSegmentsVM == null) throw new ArgumentNullException();
            if (workoutVM.WorkoutSegmentsVM[segment].SetsVM == null) return View("Error");
            if (workoutVM.WorkoutSegmentsVM[segment].SetsVM[set] == null) return View("Error");
            workoutVM.WorkoutSegmentsVM[segment].SetsVM.RemoveAt(set);
            ModelState.Clear();
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


        //<-----------------------   Create   ---------------------> 
        public async Task<IActionResult> Create(WorkoutVM? workoutVM)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            workoutVM ??= new WorkoutVM()
            {
                StartDate = DateTime.Now,
            };
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            workoutVM.Excercises = _context.Excercises.ToList();
            workoutVM.WorkoutSegmentsVM ??= new List<WorkoutSegmentVM>()
                {
                    new WorkoutSegmentVM()
                    {
                        SetsVM = new List<SetVM>{ new SetVM() }
                    }
                };
            workoutVM.ActionName = "create";
            return View(workoutVM);

        }
        [HttpPost]
        public IActionResult CreatePost(WorkoutVM workoutVM)
        {
            if (!ModelState.IsValid)
            {
                return View(workoutVM);
            }

            Workout workout = new Workout()
            {

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
                        Order = segment.Order,
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
