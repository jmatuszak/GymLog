using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace GymLog.Controllers
{
    public class ExerciseController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ExerciseController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return RedirectToAction("Login", "Account");
            
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                if (role == "user")
                {
                    RedirectToAction("Index", "Home");
                }
                else if (role == "admin")
                {
                    var Exercises = new List<Exercise>();
                    Exercises = _context.Exercises.Include(b => b.BodyParts).ToList();
                    return View(Exercises);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Create()
        {

            var bodyParts = _context.BodyParts.ToList();
            var checkedBodyPartsVM = new List<BodyPartVM>();
            foreach (var bodyPart in bodyParts)
            {
                checkedBodyPartsVM.Add(new BodyPartVM() { Id= bodyPart.Id, Name=bodyPart.Name, IsChecked=false});
            }
            var createExerciseVM = new ExerciseVM() { BodyPartsVM = checkedBodyPartsVM};
            return View(createExerciseVM);
        }

        [HttpPost]
        public IActionResult CreatePost(ExerciseVM ExerciseVM)
        {
            if (!ModelState.IsValid)
            {
                return View(ExerciseVM);
            }
            Exercise Exercise = new Exercise() 
            {
                Id = ExerciseVM.Id,
                Name= ExerciseVM.Name
            };
            _context.Exercises.Add(Exercise);
            _context.SaveChanges();
            if (ExerciseVM.BodyPartsVM != null)
            {
                foreach (var bodyPart in ExerciseVM.BodyPartsVM)
                {
                    if (bodyPart.IsChecked == true)
                    {
                        BodyPartExercise bodyPartExercise = new BodyPartExercise()
                        {
                            BodyPartId = bodyPart.Id,
                            ExerciseId = Exercise.Id
                        };
                        _context.BodyPartExercises.Add(bodyPartExercise);
                        _context.SaveChanges();
                    }
                    
                    
                }
            }
            return RedirectToAction("Index","Home");
        }



		public async Task<IActionResult> Delete(int id)
        {
            var Exercise = await _context.Exercises.FirstOrDefaultAsync(i => i.Id == id);
            if (Exercise == null) return View("Error");
            return View(Exercise);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteExercise(Exercise Exercise)
        {
            _context.Remove(Exercise);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Exercise = await _context.Exercises.Include(b => b.BodyParts).FirstOrDefaultAsync(e => e.Id == id);
            var bodyParts =  _context.BodyParts.ToList();
            if (Exercise == null) return View("Error");
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
            if(Exercise.BodyParts != null)
            foreach(var part in Exercise.BodyParts)
            {
                    bodyPartsVM.FirstOrDefault(i => i.Id == part.Id).IsChecked = true;
            }
            var ExerciseVM = new ExerciseVM()
            {
                Id = id,
                Name = Exercise.Name,
                BodyPartsVM = bodyPartsVM,
			};
			return View(ExerciseVM);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(ExerciseVM ExerciseVM)
        {
            if (!ModelState.IsValid)
            {
                return View(ExerciseVM);
            }
            var Exercise = await _context.Exercises.FirstOrDefaultAsync(i=>i.Id == ExerciseVM.Id);
            if(Exercise == null) return View("Error");
            Exercise.Name = ExerciseVM.Name;
            _context.Update(Exercise);
             var bodyPartExercises = _context.BodyPartExercises.Where(i=>i.ExerciseId == ExerciseVM.Id).ToList();
            _context.RemoveRange(bodyPartExercises);
            if (ExerciseVM.BodyPartsVM != null)
            {
                foreach (var bodyPart in ExerciseVM.BodyPartsVM)
                {
                    if (bodyPart.IsChecked == true)
                    {
                        BodyPartExercise bodyPartExercise = new BodyPartExercise()
                        {
                            BodyPartId = bodyPart.Id,
                            ExerciseId = Exercise.Id
                        };
                        _context.BodyPartExercises.Add(bodyPartExercise);
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

