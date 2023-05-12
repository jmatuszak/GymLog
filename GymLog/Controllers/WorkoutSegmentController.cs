using GymLog.Data;
using GymLog.Data.Enum;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace GymLog.Controllers
{
	public class WorkoutSegmentController : Controller
	{
		private readonly AppDbContext _context;

		public WorkoutSegmentController(AppDbContext context)
		{
			_context = context;
		}

        public IActionResult Index()
		{
			var list = _context.WorkoutSegments.Include(s=> s.Sets).Include(a => a.Excercise).Include(a => a.Template).ToList();
			return View(list);
		}

        public IActionResult AddSet(WorkoutSegmentVM WorkoutSegmentVM)
        {
            
            WorkoutSegmentVM.SetsVM ??= new List<SetVM>();
            WorkoutSegmentVM.SetsVM.Add(new SetVM());
            return View(WorkoutSegmentVM.ActionName, WorkoutSegmentVM);

        }
        public IActionResult RemoveSet(WorkoutSegmentVM WorkoutSegmentVM)
        {
            WorkoutSegmentVM.SetsVM ??= new List<SetVM>();
            WorkoutSegmentVM.SetsVM.RemoveAt(WorkoutSegmentVM.SetsVM.Count - 1);
            return View(WorkoutSegmentVM.ActionName, WorkoutSegmentVM);
        }

        //<-----------------------  CREATE   --------------------->
        public IActionResult Create(WorkoutSegmentVM? WorkoutSegmentVM)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            WorkoutSegmentVM ??= new WorkoutSegmentVM();
            WorkoutSegmentVM.SetsVM??= new List<SetVM>();
            WorkoutSegmentVM.SetsVM.Add(new SetVM());
            WorkoutSegmentVM.ActionName = "Create";
            WorkoutSegmentVM.Excercises =  _context.Excercises.ToList();
            return View(WorkoutSegmentVM);
        }

        [HttpPost]
		public IActionResult CreatePost(WorkoutSegmentVM WorkoutSegmentVM)
		{
			if(!ModelState.IsValid)
			{
				return View("Error");
			}

            var sets = new List<Set>();
            if (WorkoutSegmentVM.SetsVM != null)
                foreach (var item in WorkoutSegmentVM.SetsVM)
                {
                    var set = new Set()
                    {
                        Weight = item.Weight,
                        Reps = item.Reps,
                        WorkoutSegmentId = item.WorkoutSegmentId
                    };
                    sets.Add(set);
                }
            var WorkoutSegment = new WorkoutSegment()
            {
                Description = WorkoutSegmentVM.Description,
                ExcerciseId = WorkoutSegmentVM.ExcerciseId,
                TemplateId = WorkoutSegmentVM.TemplateId,
                Sets = sets,
                WeightType = WorkoutSegmentVM.WeightType,
            };
            _context.WorkoutSegments.Add(WorkoutSegment);
            _context.SaveChanges();
            return RedirectToAction("Index");
		}



        //<-----------------------  UPDATE   --------------------->   


        public async Task<IActionResult> Edit(int id)
		{
            var excercises = _context.Excercises.ToList();
            //var excercisesConcatVM = await CreateExcerciseConcatList();
            
            var WorkoutSegment  = await _context.WorkoutSegments.Include(x=>x.Sets).FirstOrDefaultAsync(x => x.Id == id);
			var setsVM = new List<SetVM>();
            foreach (var item in WorkoutSegment.Sets)
            {
                SetVM setVM = new SetVM()
                {
                    Id = item.Id,
                    Weight = item.Weight,
                    Reps = item.Reps,
                    WorkoutSegmentId = item.WorkoutSegmentId,
                };
                setsVM.Add(setVM);
            }
			var WorkoutSegmentVM = new WorkoutSegmentVM()
			{
				Id = WorkoutSegment.Id,
				Description = WorkoutSegment.Description,
				TemplateId = WorkoutSegment.TemplateId,
				ExcerciseId = WorkoutSegment.ExcerciseId,
                Excercises = excercises,
				SetsVM = setsVM
			};

            WorkoutSegmentVM.ActionName = "Edit";
            return View(WorkoutSegmentVM);
		}

        [HttpPost]
        public async Task<IActionResult> EditPost(WorkoutSegmentVM WorkoutSegmentVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Set. Model Error.");
                return View(WorkoutSegmentVM);
            }
            var oldSets = _context.Sets.Where(x => x.WorkoutSegmentId == WorkoutSegmentVM.Id).ToList();
            _context.Sets.RemoveRange(oldSets);

            var WorkoutSegment = await _context.WorkoutSegments.FirstOrDefaultAsync(x => x.Id == WorkoutSegmentVM.Id);
			var sets = new List<Set>();
            foreach (var item in WorkoutSegmentVM.SetsVM)
            {
                Set set = new Set()
                {
                    Weight = item.Weight,
                    Reps = item.Reps,
                    WorkoutSegmentId = item.WorkoutSegmentId,
                };
                sets.Add(set);
            }
            if (WorkoutSegment != null)
            {
                WorkoutSegment.Id = WorkoutSegmentVM.Id;
                WorkoutSegment.Description = WorkoutSegmentVM.Description;
                WorkoutSegment.ExcerciseId = WorkoutSegmentVM.ExcerciseId;
                WorkoutSegment.TemplateId = WorkoutSegmentVM.TemplateId;
                WorkoutSegment.Sets = sets;
                _context.Update(WorkoutSegment);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //<-----------------------  DELETE   ---------------------> 

        public async Task<IActionResult> Delete(int id)
        {
            var WorkoutSegment = await _context.WorkoutSegments.FirstOrDefaultAsync(x => x.Id == id);
            if (WorkoutSegment != null)
            {
                _context.WorkoutSegments.Remove(WorkoutSegment);
                var sets = _context.Sets.Where(x => x.WorkoutSegmentId == id).ToList();
                if (sets.Any()) _context.RemoveRange(sets);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
