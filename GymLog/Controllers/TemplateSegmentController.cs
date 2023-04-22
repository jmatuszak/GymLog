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
	public class TemplateSegmentController : Controller
	{
		private readonly AppDbContext _context;

		public TemplateSegmentController(AppDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var list = _context.TemplateSegments.Include(s=> s.Sets).Include(a => a.Excercise).Include(a => a.Template).ToList();
			return View(list);
		}

        public IActionResult AddSet(TemplateSegmentVM templateSegmentVM)
        {
            
            templateSegmentVM.SetsVM ??= new List<SetVM>();
            templateSegmentVM.SetsVM.Add(new SetVM());
            return View(templateSegmentVM.ActionName, templateSegmentVM);

        }
        public IActionResult RemoveSet(TemplateSegmentVM templateSegmentVM)
        {
            templateSegmentVM.SetsVM ??= new List<SetVM>();
            templateSegmentVM.SetsVM.RemoveAt(templateSegmentVM.SetsVM.Count - 1);
            return View(templateSegmentVM.ActionName, templateSegmentVM);
        }

        //<-----------------------  CREATE   --------------------->
        public IActionResult Create(TemplateSegmentVM? templateSegmentVM)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            templateSegmentVM ??= new TemplateSegmentVM();
            templateSegmentVM.SetsVM??= new List<SetVM>();
            templateSegmentVM.SetsVM.Add(new SetVM());
            templateSegmentVM.ActionName = "Create";
            templateSegmentVM.Excercises =  _context.Excercises.ToList();
            return View(templateSegmentVM);
        }

        [HttpPost]
		public IActionResult CreatePost(TemplateSegmentVM templateSegmentVM)
		{
			if(!ModelState.IsValid)
			{
				return View("Error");
			}

            var sets = new List<Set>();
            if (templateSegmentVM.SetsVM != null)
                foreach (var item in templateSegmentVM.SetsVM)
                {
                    var set = new Set()
                    {
                        Weight = item.Weight,
                        Reps = item.Reps,
                        TemplateSegmentId = item.TemplateSegmentId
                    };
                    sets.Add(set);
                }
            
            var templateSegment = new TemplateSegment()
            {
                Order = templateSegmentVM.Order,
                Description = templateSegmentVM.Description,
                ExcerciseId = templateSegmentVM.ExcerciseId,
                TemplateId = templateSegmentVM.TemplateId,
                Sets = sets
            };
            _context.TemplateSegments.Add(templateSegment);
            _context.SaveChanges();
            return RedirectToAction("Index");
		}



        //<-----------------------  UPDATE   --------------------->   


        public async Task<IActionResult> Edit(int id)
		{
            var excercises = _context.Excercises.ToList();
            //var excercisesConcatVM = await CreateExcerciseConcatList();
            
            var templateSegment  = await _context.TemplateSegments.Include(x=>x.Sets).FirstOrDefaultAsync(x => x.Id == id);
			var setsVM = new List<SetVM>();
            foreach (var item in templateSegment.Sets)
            {
                SetVM setVM = new SetVM()
                {
                    Id = item.Id,
                    Weight = item.Weight,
                    Reps = item.Reps,
                    TemplateSegmentId = item.TemplateSegmentId,
                };
                setsVM.Add(setVM);
            }
			var templateSegmentVM = new TemplateSegmentVM()
			{
				Id = templateSegment.Id,
				Order = templateSegment.Order,
				Description = templateSegment.Description,
				TemplateId = templateSegment.TemplateId,
				ExcerciseId = templateSegment.ExcerciseId,
                Excercises = excercises,
				//Excercises = excercisesConcatVM,
				SetsVM = setsVM
			};

            templateSegmentVM.ActionName = "Edit";
            return View(templateSegmentVM);
		}

        [HttpPost]
        public async Task<IActionResult> EditPost(TemplateSegmentVM templateSegmentVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Set. Model Error.");
                return View(templateSegmentVM);
            }
            var oldSets = _context.Sets.Where(x => x.TemplateSegmentId == templateSegmentVM.Id).ToList();
            _context.Sets.RemoveRange(oldSets);

            var templateSegment = await _context.TemplateSegments.FirstOrDefaultAsync(x => x.Id == templateSegmentVM.Id);
			var sets = new List<Set>();
            foreach (var item in templateSegmentVM.SetsVM)
            {
                Set set = new Set()
                {
                    Weight = item.Weight,
                    Reps = item.Reps,
                    TemplateSegmentId = item.TemplateSegmentId,
                };
                sets.Add(set);
            }
            if (templateSegment != null)
            {
                templateSegment.Id = templateSegmentVM.Id;
                templateSegment.Order = templateSegmentVM.Order;
                templateSegment.Description = templateSegmentVM.Description;
                templateSegment.ExcerciseId = templateSegmentVM.ExcerciseId;
                templateSegment.TemplateId = templateSegmentVM.TemplateId;
                templateSegment.Sets = sets;
                _context.Update(templateSegment);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //<-----------------------  DELETE   ---------------------> 

        public async Task<IActionResult> DeleteTemplateSegment(int id)
        {
            var TemplateSegment = await _context.TemplateSegments.FirstOrDefaultAsync(x => x.Id == id);
            if (TemplateSegment != null)
            {
                _context.TemplateSegments.Remove(TemplateSegment);
            }
            var sets = _context.Sets.Where(x => x.TemplateSegmentId== id).ToList();
            if(sets.Any()) _context.RemoveRange(sets);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
