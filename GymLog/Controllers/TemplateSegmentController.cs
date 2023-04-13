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



		public async Task<List<CreateExcerciseConcatVM>> CreateExcerciseConcatList()
		{
			var excercises = await _context.Excercises.ToListAsync();
			var excercisesVM = new List<CreateExcerciseConcatVM>();

			foreach (var item in excercises)
			{
				excercisesVM.Add(new CreateExcerciseConcatVM()
				{
					Id = item.Id,
					Name = item.Name + " - " + item.WeightType,
				});
			}
            return excercisesVM;
        }
        public TemplateSegmentVM AddSet(TemplateSegmentVM TemplateSegmentVM)
        {
            if (TemplateSegmentVM.SetsVM == null) TemplateSegmentVM.SetsVM = new List<SetVM>();
            TemplateSegmentVM.SetsVM.Add(new SetVM());
            return TemplateSegmentVM;

        }
        public TemplateSegmentVM RemoveSet(TemplateSegmentVM TemplateSegmentVM)
        {
            if (TemplateSegmentVM.SetsVM.Count < 1) return TemplateSegmentVM;
            TemplateSegmentVM.SetsVM.RemoveAt(TemplateSegmentVM.SetsVM.Count - 1);
            return TemplateSegmentVM;
        }


        //<-----------------------  CREATE   --------------------->   


        public IActionResult AddSetCreate(TemplateSegmentVM TemplateSegmentVM)
        {
            if (TemplateSegmentVM.SetsVM == null) TemplateSegmentVM.SetsVM = new List<SetVM>();
            TemplateSegmentVM = AddSet(TemplateSegmentVM);
            return View("Create", TemplateSegmentVM);

        }
        public IActionResult RemoveSetCreate(TemplateSegmentVM TemplateSegmentVM)
        {
            if (TemplateSegmentVM.SetsVM == null) return View("Error");
            TemplateSegmentVM = RemoveSet(TemplateSegmentVM);
            return View("Create", TemplateSegmentVM);
        }


        public async Task<IActionResult> Create(TemplateSegmentVM? TemplateSegmentVM)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            if (TemplateSegmentVM.ExcercisesConcatVM == null)
            {
                var excercisesConcatVM = await CreateExcerciseConcatList();
                TemplateSegmentVM.ExcercisesConcatVM = excercisesConcatVM;
            }
            TemplateSegmentVM = AddSet(TemplateSegmentVM);
            return View(TemplateSegmentVM);
        }

        [HttpPost]
		public async Task<IActionResult> CreateTemplateSegment(TemplateSegmentVM TemplateSegmentVM)
		{
			if(!ModelState.IsValid)
			{
				return View("Error");
			}
            var sets = new List<Set>();
            if (TemplateSegmentVM.SetsVM != null)
			{
				foreach (var item in TemplateSegmentVM.SetsVM)
				{
					var set = new Set() 
					{
						Weight = item.Weight,
						Reps = item.Reps,
						TemplateSegmentId = item.TemplateSegmentId	
					};
					sets.Add(set);
				}
				var TemplateSegment = new TemplateSegment()
				{
					Order = TemplateSegmentVM.Order,
					Description = TemplateSegmentVM.Description,
					ExcerciseId = TemplateSegmentVM.ExcerciseId,
					TemplateId = TemplateSegmentVM.TemplateId,
					Sets = sets
				};
                _context.TemplateSegments.Add(TemplateSegment);
                _context.SaveChanges();
            }
			else
			{
                var TemplateSegment = new TemplateSegment()
                {
                    Order = TemplateSegmentVM.Order,
                    Description = TemplateSegmentVM.Description,
                    ExcerciseId = TemplateSegmentVM.ExcerciseId,
                    TemplateId = TemplateSegmentVM.TemplateId,
                };
                _context.TemplateSegments.Add(TemplateSegment);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
		}



        //<-----------------------  UPDATE   --------------------->   


        public IActionResult AddSetEdit(TemplateSegmentVM TemplateSegmentVM)
        {
            if (TemplateSegmentVM.SetsVM == null) TemplateSegmentVM.SetsVM = new List<SetVM>();
            TemplateSegmentVM = AddSet(TemplateSegmentVM);
            return View("Edit", TemplateSegmentVM);

        }
        public IActionResult RemoveSetEdit(TemplateSegmentVM TemplateSegmentVM)
        {
            if (TemplateSegmentVM.SetsVM == null) return View("Error");
            TemplateSegmentVM = RemoveSet(TemplateSegmentVM);
            return View("Edit", TemplateSegmentVM);
        }


        public async Task<IActionResult> Edit(int id)
		{
            var excercisesConcatVM = await CreateExcerciseConcatList();
            
            var TemplateSegment  = await _context.TemplateSegments.Include(x=>x.Sets).FirstOrDefaultAsync(x => x.Id == id);
			var setsVM = new List<SetVM>();
            foreach (var item in TemplateSegment.Sets)
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
			var TemplateSegmentVM = new TemplateSegmentVM()
			{
				Id = TemplateSegment.Id,
				Order = TemplateSegment.Order,
				Description = TemplateSegment.Description,
				TemplateId = TemplateSegment.TemplateId,
				ExcerciseId = TemplateSegment.ExcerciseId,
				ExcercisesConcatVM = excercisesConcatVM,
				SetsVM = setsVM
			};

			return View(TemplateSegmentVM);
		}

        [HttpPost]
        public async Task<IActionResult> Edit(TemplateSegmentVM TemplateSegmentVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Set");
                return View(TemplateSegmentVM);
            }
            var oldSets = _context.Sets.Where(x => x.TemplateSegmentId == TemplateSegmentVM.Id).ToList();
            _context.Sets.RemoveRange(oldSets);

            var TemplateSegment = await _context.TemplateSegments.FirstOrDefaultAsync(x => x.Id == TemplateSegmentVM.Id);
			var sets = new List<Set>();
            foreach (var item in TemplateSegmentVM.SetsVM)
            {
                Set set = new Set()
                {
                    Weight = item.Weight,
                    Reps = item.Reps,
                    TemplateSegmentId = item.TemplateSegmentId,
                };
                sets.Add(set);
            }
            if (TemplateSegment != null)
            {
                TemplateSegment.Id = TemplateSegmentVM.Id;
                TemplateSegment.Order = TemplateSegmentVM.Order;
                TemplateSegment.Description = TemplateSegmentVM.Description;
                TemplateSegment.ExcerciseId = TemplateSegmentVM.ExcerciseId;
                TemplateSegment.TemplateId = TemplateSegmentVM.TemplateId;
                TemplateSegment.Sets = sets;
                _context.Update(TemplateSegment);
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
