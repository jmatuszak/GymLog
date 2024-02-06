using GymLog.Data;
using GymLog.Interfaces;
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
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseController(AppDbContext context, UserManager<AppUser> userManager, IExerciseRepository exerciseRepository)
        {
            _context = context;
            _userManager = userManager;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                var exercises = await _exerciseRepository.GetExerciseListAsync();
                return View(exercises);
            }
            else return RedirectToAction("Login", "Account");
        }


        //CREATE
        public IActionResult Create()
        {
            if (User.IsInRole("user") || User.IsInRole("admin"))
            {
                var bodyParts = _context.BodyParts.ToList();
                var checkedBodyPartsVM = new List<BodyPartVM>();
                foreach (var bodyPart in bodyParts)
                {
                    checkedBodyPartsVM.Add(new BodyPartVM() { Id = bodyPart.Id, Name = bodyPart.Name, IsChecked = false });
                }
                var createExerciseVM = new ExerciseVM() { BodyPartsVM = checkedBodyPartsVM };
                return View(createExerciseVM);
            }
            else
                return RedirectToAction("Login", "Account");

        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(ExerciseVM exerciseVM)
        {
            if (!ModelState.IsValid) return View("Create", exerciseVM);

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var exercise = ConvertVmToExercise(exerciseVM);
            
            if (User.IsInRole("user"))
                exercise.AppUserId = user.Id;

            _exerciseRepository.InsertExercise(exercise);

            exercise.BodyParts = GetBodyParts(exerciseVM);
            _exerciseRepository.InsertBodyPartExercise(exercise);

            return RedirectToAction("Index","Home");
        }


        //DELETE
		public async Task<IActionResult> Delete(int id)
        {
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);
            if (exercise == null) return View("Error");
            return View(exercise);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteExercise(Exercise exercise)
        {
            _exerciseRepository.DeleteExercise(exercise);
            return RedirectToAction("Index");
        }


        //UPDATE
        public async Task<IActionResult> Edit(int id)
        {
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);

            var exerciseVM = PrepareExerciseVMToUpdate(exercise);
            
			return View(exerciseVM);
        }

        [HttpPost]
        public IActionResult Edit(ExerciseVM exerciseVM)
        {
            if (!ModelState.IsValid) return View(exerciseVM);

            var exercise = ConvertVmToExercise(exerciseVM);

            _exerciseRepository.UpdateExercise(exercise);

            exercise.BodyParts = GetBodyParts(exerciseVM);
            _exerciseRepository.UpdateBodyPartExercise(exercise);
            
            return RedirectToAction("Index");
        }
    }
}

