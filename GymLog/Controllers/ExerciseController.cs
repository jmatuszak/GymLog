using GymLog.Interfaces;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymLog.Controllers
{
    public class ExerciseController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IBodyPartRepository _bodyPartRepository;

        public ExerciseController(UserManager<AppUser> userManager, IExerciseRepository exerciseRepository, IBodyPartRepository bodyPartRepository)
        {
            _userManager = userManager;
            _exerciseRepository = exerciseRepository;
            _bodyPartRepository = bodyPartRepository;
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
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("user") || User.IsInRole("admin"))
            {
                var bodyParts = await _bodyPartRepository.GetBodyPartListAsync();
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
            _exerciseRepository.Save();

            exercise.BodyParts = GetCheckedBodyParts(exerciseVM);
            _exerciseRepository.InsertBodyPartExercise(exercise);
            _exerciseRepository.Save();


            return RedirectToAction("Index", "Home");
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
            _exerciseRepository.Save();
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
            _exerciseRepository.Save();

            exercise.BodyParts = GetCheckedBodyParts(exerciseVM);
            _exerciseRepository.UpdateBodyPartExercise(exercise);
            _exerciseRepository.Save();
            return RedirectToAction("Index");
        }
    }
}

