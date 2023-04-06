using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Controllers
{
    public class ExcerciseController : Controller
    {
        private readonly AppDbContext _context;

        public ExcerciseController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var excercises = new List<Excercise>();
            excercises = _context.Excercises.ToList();
            return View(excercises);
        }
        public IActionResult Create()
        {

            var bodyParts = _context.BodyParts.ToList();
            var checkedBodyPartsVM = new List<BodyPartVM>();
            foreach (var bodyPart in bodyParts)
            {
                checkedBodyPartsVM.Add(new BodyPartVM() { Id= bodyPart.Id, Name=bodyPart.Name, IsChecked=false});
            }
            var createExcerciseVM = new CreateExcerciseVM() { BodyPartsVM = checkedBodyPartsVM};
            return View(createExcerciseVM);
        }

        public IActionResult CreateNew(CreateExcerciseVM excerciseVM)
        {
            return View(excerciseVM);
        }



        [HttpPost]
        public async Task<IActionResult> Create(CreateExcerciseVM excerciseVM)
        {
            if (!ModelState.IsValid)
            {
                return View(excerciseVM);
            }
            Excercise excercise = new Excercise() 
            {
                Id = excerciseVM.Id,
                Name= excerciseVM.Name
            };
            _context.Excercises.Add(excercise);
            _context.SaveChanges();
            if (excerciseVM.BodyPartsVM != null)
            {
                foreach (var bodyPart in excerciseVM.BodyPartsVM)
                {
                    if (bodyPart.IsChecked == true)
                    {
                        BodyPartExcercise bodyPartExcercise = new BodyPartExcercise()
                        {
                            BodyPartId = bodyPart.Id,
                            ExcerciseId = excercise.Id,
                            BodyPart = _context.BodyParts.Where(x => x.Id == bodyPart.Id).FirstOrDefault(),
                            Excercise = excercise
                        };
                        _context.BodyPartExcercises.Add(bodyPartExcercise);
                        _context.SaveChanges();
                    }
                    
                    
                }
            }
            return RedirectToAction("Index");
        }



		public async Task<IActionResult> Delete(int id)
        {
            var excercise = await _context.Excercises.FirstOrDefaultAsync(i => i.Id == id);
            if (excercise == null) return View("Error");
            return View(excercise);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteExcercise(Excercise excercise)
        {
            _context.Remove(excercise);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

		/*public async Task<IActionResult> Edit(int id)
		{
			var excercise = await _context.Excercises.Include(b => b.BodyParts).FirstOrDefaultAsync(e => e.Id == id);
			if (excercise == null) return View("Error");
			var excerciseVM = new CreateExcerciseVM()
			{
				Id = id,
				Name = excercise.Name,
				WeightType = excercise.WeightType,

				if (excercise.BodyParts != null) BodyPartsVM = excercise.BodyParts,
			};
			return View(setVM);
	}
	[HttpPost]
	public async Task<IActionResult> Edit(SetVM setVM)
	{
		if (!ModelState.IsValid)
		{
			ModelState.AddModelError("", "Failed to edit Set");
			return View(setVM);
		}
		var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == setVM.Id);

		if (set != null)
		{
			set.Weight = setVM.Weight;
			set.Reps = setVM.Reps;
			set.Description = setVM.Description;
			set.ExcerciseSetId = setVM.ExcerciseSetId;
			_context.Update(set);
			_context.SaveChanges();
		}
		return RedirectToAction("Index");
	}*/

}
}
