using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace GymLog.Controllers
{
    public class ExcerciseController : BaseController
    {
        private readonly AppDbContext _context;

        public ExcerciseController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var excercises = new List<Excercise>();
            excercises = _context.Excercises.Include(b=>b.BodyParts).ToList();
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
            var createExcerciseVM = new ExcerciseVM() { BodyPartsVM = checkedBodyPartsVM};
            return View(createExcerciseVM);
        }

        [HttpPost]
        public IActionResult CreatePost(ExcerciseVM excerciseVM)
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
                            ExcerciseId = excercise.Id
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

        public async Task<IActionResult> Edit(int id)
        {
            var excercise = await _context.Excercises.Include(b => b.BodyParts).FirstOrDefaultAsync(e => e.Id == id);
            var bodyParts =  _context.BodyParts.ToList();
            if (excercise == null) return View("Error");
            var bodyPartsVM = new List<BodyPartVM>();
            if (bodyParts != null) 
                foreach(var part in bodyParts)
                {
                    bodyPartsVM.Add(new BodyPartVM
                    {
                        Id = part.Id,
                        Name = part.Name,
                    });
                }
            if(excercise.BodyParts != null)
            foreach(var part in excercise.BodyParts)
            {
                    bodyPartsVM.FirstOrDefault(i => i.Id == part.Id).IsChecked = true;
            }
            var excerciseVM = new ExcerciseVM()
            {
                Id = id,
                Name = excercise.Name,
                BodyPartsVM = bodyPartsVM,
			};
			return View(excerciseVM);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(ExcerciseVM excerciseVM)
        {
            if (!ModelState.IsValid)
            {
                return View(excerciseVM);
            }
            var excercise = await _context.Excercises.FirstOrDefaultAsync(i=>i.Id == excerciseVM.Id);
            if(excercise == null) return View("Error");
            excercise.Name = excerciseVM.Name;
            _context.Update(excercise);
             var bodyPartExcercises = _context.BodyPartExcercises.Where(i=>i.ExcerciseId == excerciseVM.Id).ToList();
            _context.RemoveRange(bodyPartExcercises);
            if (excerciseVM.BodyPartsVM != null)
            {
                foreach (var bodyPart in excerciseVM.BodyPartsVM)
                {
                    if (bodyPart.IsChecked == true)
                    {
                        BodyPartExcercise bodyPartExcercise = new BodyPartExcercise()
                        {
                            BodyPartId = bodyPart.Id,
                            ExcerciseId = excercise.Id
                        };
                        _context.BodyPartExcercises.Add(bodyPartExcercise);
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> FindExcercise(FindExcerciseVM? findExcerciseVM)
        {
            findExcerciseVM ??= new FindExcerciseVM();
            findExcerciseVM.ExcercisesVM ??= new List<ExcerciseVM>();
            findExcerciseVM.SearchedExcercisesVM ??= new List<ExcerciseVM>();


			var excercises = _context.Excercises.ToList();

			foreach (var excercise in excercises)
            {
                var excerciseVM = ExcerciseToExcerciseVM(excercise, new ExcerciseVM());
				findExcerciseVM.ExcercisesVM.Add(excerciseVM);
			}
            if (findExcerciseVM.SearchString == null) return View(findExcerciseVM);
			var searchedExcercises = await _context.Excercises.Where(x => x.Name.Contains(findExcerciseVM.SearchString)).ToListAsync();
			foreach (var excercise in searchedExcercises)
			{
				var excerciseVM = ExcerciseToExcerciseVM(excercise, new ExcerciseVM());
				findExcerciseVM.SearchedExcercisesVM.Add(excerciseVM);
			}

			return View(findExcerciseVM);
		}

        
        public async Task<IActionResult> FindExcerciseModal(FindExcerciseVM? findExcerciseVM)
        {
            findExcerciseVM ??= new FindExcerciseVM();
            findExcerciseVM.ExcercisesVM ??= new List<ExcerciseVM>();
            findExcerciseVM.SearchedExcercisesVM ??= new List<ExcerciseVM>();


            var excercises = _context.Excercises.ToList();

            foreach (var excercise in excercises)
            {
                var excerciseVM = ExcerciseToExcerciseVM(excercise, new ExcerciseVM());
                findExcerciseVM.ExcercisesVM.Add(excerciseVM);
            }
            if (findExcerciseVM.SearchString == null) return View(findExcerciseVM);
            var searchedExcercises = await _context.Excercises.Where(x => x.Name.Contains(findExcerciseVM.SearchString)).ToListAsync();
            foreach (var excercise in searchedExcercises)
            {
                var excerciseVM = ExcerciseToExcerciseVM(excercise, new ExcerciseVM());
                findExcerciseVM.SearchedExcercisesVM.Add(excerciseVM);
            }

            return PartialView(findExcerciseVM);
        }

    }
}
