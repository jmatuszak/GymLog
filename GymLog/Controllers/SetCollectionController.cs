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
	public class SetCollectionController : Controller
	{
		private readonly AppDbContext _context;

		public SetCollectionController(AppDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var list = _context.SetCollections.Include(s=> s.Sets).Include(a => a.Excercise).Include(a => a.Template).ToList();
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
        public SetCollectionVM AddSet(SetCollectionVM setCollectionVM)
        {
            if (setCollectionVM.SetsVM == null) setCollectionVM.SetsVM = new List<SetVM>();
            setCollectionVM.SetsVM.Add(new SetVM());
            return setCollectionVM;

        }
        public SetCollectionVM RemoveSet(SetCollectionVM setCollectionVM)
        {
            if (setCollectionVM.SetsVM.Count < 1) return setCollectionVM;
            setCollectionVM.SetsVM.RemoveAt(setCollectionVM.SetsVM.Count - 1);
            return setCollectionVM;
        }


        //<-----------------------  CREATE   --------------------->   


        public IActionResult AddSetCreate(SetCollectionVM setCollectionVM)
        {
            if (setCollectionVM.SetsVM == null) setCollectionVM.SetsVM = new List<SetVM>();
            setCollectionVM = AddSet(setCollectionVM);
            return View("Create", setCollectionVM);

        }
        public IActionResult RemoveSetCreate(SetCollectionVM setCollectionVM)
        {
            if (setCollectionVM.SetsVM == null) return View("Error");
            setCollectionVM = RemoveSet(setCollectionVM);
            return View("Create", setCollectionVM);
        }


        public async Task<IActionResult> Create(SetCollectionVM? setCollectionVM)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            if (setCollectionVM.ExcercisesConcatVM == null)
            {
                var excercisesConcatVM = await CreateExcerciseConcatList();
                setCollectionVM.ExcercisesConcatVM = excercisesConcatVM;
            }
            setCollectionVM = AddSet(setCollectionVM);
            return View(setCollectionVM);
        }

        [HttpPost]
		public async Task<IActionResult> CreateSetCollection(SetCollectionVM setCollectionVM)
		{
			if(!ModelState.IsValid)
			{
				return View("Error");
			}
            var sets = new List<Set>();
            if (setCollectionVM.SetsVM != null)
			{
				foreach (var item in setCollectionVM.SetsVM)
				{
					var set = new Set() 
					{
						Weight = item.Weight,
						Reps = item.Reps,
						SetCollectionId = item.SetCollectionId	
					};
					sets.Add(set);
				}
				var setCollection = new SetCollection()
				{
					Order = setCollectionVM.Order,
					Description = setCollectionVM.Description,
					ExcerciseId = setCollectionVM.ExcerciseId,
					TemplateId = setCollectionVM.TemplateId,
					Sets = sets
				};
                _context.SetCollections.Add(setCollection);
                _context.SaveChanges();
            }
			else
			{
                var setCollection = new SetCollection()
                {
                    Order = setCollectionVM.Order,
                    Description = setCollectionVM.Description,
                    ExcerciseId = setCollectionVM.ExcerciseId,
                    TemplateId = setCollectionVM.TemplateId,
                };
                _context.SetCollections.Add(setCollection);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
		}



        //<-----------------------  UPDATE   --------------------->   


        public IActionResult AddSetEdit(SetCollectionVM setCollectionVM)
        {
            if (setCollectionVM.SetsVM == null) setCollectionVM.SetsVM = new List<SetVM>();
            setCollectionVM = AddSet(setCollectionVM);
            return View("Edit", setCollectionVM);

        }
        public IActionResult RemoveSetEdit(SetCollectionVM setCollectionVM)
        {
            if (setCollectionVM.SetsVM == null) return View("Error");
            setCollectionVM = RemoveSet(setCollectionVM);
            return View("Edit", setCollectionVM);
        }


        public async Task<IActionResult> Edit(int id)
		{
            var excercisesConcatVM = await CreateExcerciseConcatList();
            
            var setCollection  = await _context.SetCollections.Include(x=>x.Sets).FirstOrDefaultAsync(x => x.Id == id);
			var setsVM = new List<SetVM>();
            foreach (var item in setCollection.Sets)
            {
                SetVM setVM = new SetVM()
                {
                    Id = item.Id,
                    Weight = item.Weight,
                    Reps = item.Reps,
                    SetCollectionId = item.SetCollectionId,
                };
                setsVM.Add(setVM);
            }
			var setCollectionVM = new SetCollectionVM()
			{
				Id = setCollection.Id,
				Order = setCollection.Order,
				Description = setCollection.Description,
				TemplateId = setCollection.TemplateId,
				ExcerciseId = setCollection.ExcerciseId,
				ExcercisesConcatVM = excercisesConcatVM,
				SetsVM = setsVM
			};

			return View(setCollectionVM);
		}

        [HttpPost]
        public async Task<IActionResult> Edit(SetCollectionVM setCollectionVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Set");
                return View(setCollectionVM);
            }
            var oldSets = _context.Sets.Where(x => x.SetCollectionId == setCollectionVM.Id).ToList();
            _context.Sets.RemoveRange(oldSets);

            var setCollection = await _context.SetCollections.FirstOrDefaultAsync(x => x.Id == setCollectionVM.Id);
			var sets = new List<Set>();
            foreach (var item in setCollectionVM.SetsVM)
            {
                Set set = new Set()
                {
                    Weight = item.Weight,
                    Reps = item.Reps,
                    SetCollectionId = item.SetCollectionId,
                };
                sets.Add(set);
            }
            if (setCollection != null)
            {
                setCollection.Id = setCollectionVM.Id;
                setCollection.Order = setCollectionVM.Order;
                setCollection.Description = setCollectionVM.Description;
                setCollection.ExcerciseId = setCollectionVM.ExcerciseId;
                setCollection.TemplateId = setCollectionVM.TemplateId;
                setCollection.Sets = sets;
                _context.Update(setCollection);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //<-----------------------  DELETE   ---------------------> 

        public async Task<IActionResult> DeleteSetCollection(int id)
        {
            var setCollection = await _context.SetCollections.FirstOrDefaultAsync(x => x.Id == id);
            if (setCollection != null)
            {
                _context.SetCollections.Remove(setCollection);
            }
            var sets = _context.Sets.Where(x => x.SetCollectionId== id).ToList();
            if(sets.Any()) _context.RemoveRange(sets);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
