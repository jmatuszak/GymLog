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
	public class ExcerciseSetController : Controller
	{
		private readonly AppDbContext _context;

		public ExcerciseSetController(AppDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var list = _context.ExcerciseSets.Include(s=> s.Sets).Include(a => a.Excercise).Include(a => a.Template).ToList();
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
        public ExcerciseSetVM AddSet(ExcerciseSetVM ExcerciseSetVM)
        {
            if (ExcerciseSetVM.SetsVM == null) ExcerciseSetVM.SetsVM = new List<SetVM>();
            ExcerciseSetVM.SetsVM.Add(new SetVM());
            return ExcerciseSetVM;

        }
        public ExcerciseSetVM RemoveSet(ExcerciseSetVM ExcerciseSetVM)
        {
            if (ExcerciseSetVM.SetsVM.Count < 1) return ExcerciseSetVM;
            ExcerciseSetVM.SetsVM.RemoveAt(ExcerciseSetVM.SetsVM.Count - 1);
            return ExcerciseSetVM;
        }


        //<-----------------------  CREATE   --------------------->   


        public IActionResult AddSetCreate(ExcerciseSetVM ExcerciseSetVM)
        {
            if (ExcerciseSetVM.SetsVM == null) ExcerciseSetVM.SetsVM = new List<SetVM>();
            ExcerciseSetVM = AddSet(ExcerciseSetVM);
            return View("Create", ExcerciseSetVM);

        }
        public IActionResult RemoveSetCreate(ExcerciseSetVM ExcerciseSetVM)
        {
            if (ExcerciseSetVM.SetsVM == null) return View("Error");
            ExcerciseSetVM = RemoveSet(ExcerciseSetVM);
            return View("Create", ExcerciseSetVM);
        }


        public async Task<IActionResult> Create(ExcerciseSetVM? ExcerciseSetVM)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            if (ExcerciseSetVM.ExcercisesConcatVM == null)
            {
                var excercisesConcatVM = await CreateExcerciseConcatList();
                ExcerciseSetVM.ExcercisesConcatVM = excercisesConcatVM;
            }
            ExcerciseSetVM = AddSet(ExcerciseSetVM);
            return View(ExcerciseSetVM);
        }

        [HttpPost]
		public async Task<IActionResult> CreateExcerciseSet(ExcerciseSetVM ExcerciseSetVM)
		{
			if(!ModelState.IsValid)
			{
				return View("Error");
			}
            var sets = new List<Set>();
            if (ExcerciseSetVM.SetsVM != null)
			{
				foreach (var item in ExcerciseSetVM.SetsVM)
				{
					var set = new Set() 
					{
						Weight = item.Weight,
						Reps = item.Reps,
						ExcerciseSetId = item.ExcerciseSetId	
					};
					sets.Add(set);
				}
				var ExcerciseSet = new ExcerciseSet()
				{
					Order = ExcerciseSetVM.Order,
					Description = ExcerciseSetVM.Description,
					ExcerciseId = ExcerciseSetVM.ExcerciseId,
					TemplateId = ExcerciseSetVM.TemplateId,
					Sets = sets
				};
                _context.ExcerciseSets.Add(ExcerciseSet);
                _context.SaveChanges();
            }
			else
			{
                var ExcerciseSet = new ExcerciseSet()
                {
                    Order = ExcerciseSetVM.Order,
                    Description = ExcerciseSetVM.Description,
                    ExcerciseId = ExcerciseSetVM.ExcerciseId,
                    TemplateId = ExcerciseSetVM.TemplateId,
                };
                _context.ExcerciseSets.Add(ExcerciseSet);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
		}



        //<-----------------------  UPDATE   --------------------->   


        public IActionResult AddSetEdit(ExcerciseSetVM ExcerciseSetVM)
        {
            if (ExcerciseSetVM.SetsVM == null) ExcerciseSetVM.SetsVM = new List<SetVM>();
            ExcerciseSetVM = AddSet(ExcerciseSetVM);
            return View("Edit", ExcerciseSetVM);

        }
        public IActionResult RemoveSetEdit(ExcerciseSetVM ExcerciseSetVM)
        {
            if (ExcerciseSetVM.SetsVM == null) return View("Error");
            ExcerciseSetVM = RemoveSet(ExcerciseSetVM);
            return View("Edit", ExcerciseSetVM);
        }


        public async Task<IActionResult> Edit(int id)
		{
            var excercisesConcatVM = await CreateExcerciseConcatList();
            
            var ExcerciseSet  = await _context.ExcerciseSets.Include(x=>x.Sets).FirstOrDefaultAsync(x => x.Id == id);
			var setsVM = new List<SetVM>();
            foreach (var item in ExcerciseSet.Sets)
            {
                SetVM setVM = new SetVM()
                {
                    Id = item.Id,
                    Weight = item.Weight,
                    Reps = item.Reps,
                    ExcerciseSetId = item.ExcerciseSetId,
                };
                setsVM.Add(setVM);
            }
			var ExcerciseSetVM = new ExcerciseSetVM()
			{
				Id = ExcerciseSet.Id,
				Order = ExcerciseSet.Order,
				Description = ExcerciseSet.Description,
				TemplateId = ExcerciseSet.TemplateId,
				ExcerciseId = ExcerciseSet.ExcerciseId,
				ExcercisesConcatVM = excercisesConcatVM,
				SetsVM = setsVM
			};

			return View(ExcerciseSetVM);
		}

        [HttpPost]
        public async Task<IActionResult> Edit(ExcerciseSetVM ExcerciseSetVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Set");
                return View(ExcerciseSetVM);
            }
            var oldSets = _context.Sets.Where(x => x.ExcerciseSetId == ExcerciseSetVM.Id).ToList();
            _context.Sets.RemoveRange(oldSets);

            var ExcerciseSet = await _context.ExcerciseSets.FirstOrDefaultAsync(x => x.Id == ExcerciseSetVM.Id);
			var sets = new List<Set>();
            foreach (var item in ExcerciseSetVM.SetsVM)
            {
                Set set = new Set()
                {
                    Weight = item.Weight,
                    Reps = item.Reps,
                    ExcerciseSetId = item.ExcerciseSetId,
                };
                sets.Add(set);
            }
            if (ExcerciseSet != null)
            {
                ExcerciseSet.Id = ExcerciseSetVM.Id;
                ExcerciseSet.Order = ExcerciseSetVM.Order;
                ExcerciseSet.Description = ExcerciseSetVM.Description;
                ExcerciseSet.ExcerciseId = ExcerciseSetVM.ExcerciseId;
                ExcerciseSet.TemplateId = ExcerciseSetVM.TemplateId;
                ExcerciseSet.Sets = sets;
                _context.Update(ExcerciseSet);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //<-----------------------  DELETE   ---------------------> 

        public async Task<IActionResult> DeleteExcerciseSet(int id)
        {
            var ExcerciseSet = await _context.ExcerciseSets.FirstOrDefaultAsync(x => x.Id == id);
            if (ExcerciseSet != null)
            {
                _context.ExcerciseSets.Remove(ExcerciseSet);
            }
            var sets = _context.Sets.Where(x => x.ExcerciseSetId== id).ToList();
            if(sets.Any()) _context.RemoveRange(sets);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
