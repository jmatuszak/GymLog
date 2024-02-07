using GymLog.Data;
using GymLog.Interfaces;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymLog.Controllers
{
    public class SetController : Controller
    {
        private readonly ISetRepository _setRepository;

        public SetController(ISetRepository setRepository)
        {
            _setRepository = setRepository;
        }
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                var sets = await _setRepository.GetSetListAsync();
                return View(sets);
            }
            else return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                var sets = await _setRepository.GetSetListAsync();
                return View();
            }
            else return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Create(Set set)
        {
            if (!ModelState.IsValid) return View(set);

            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                _setRepository.InsertSet(set);
                _setRepository.Save();
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                var set = await _setRepository.GetSetByIdAsync(id);

                var setVM = new SetVM()
                {
                    Id = id,
                    Weight = set.Weight,
                    Reps = set.Reps,
                    Description = set.Description,
                    WorkoutSegmentId = set.WorkoutSegmentId,
                };
                return View(setVM);
            }
            else return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(SetVM setVM)
        {
            if (!ModelState.IsValid) return View(setVM);

            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                var set = await _setRepository.GetSetByIdAsync(setVM.Id);

                if (set != null)
                {
                    set.Weight = setVM.Weight;
                    set.Reps = setVM.Reps;
                    set.Description = setVM.Description;
                    set.WorkoutSegmentId = setVM.WorkoutSegmentId;
                    _setRepository.UpdateSet(set);
                    _setRepository.Save();
                }
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                var set = await _setRepository.GetSetByIdAsync(id);
                if (set == null) return View("Error");
                return View(set);
            }
            else return RedirectToAction("Login", "Account");
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteSet(Set set)
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                _setRepository.DeleteSet(set);
                _setRepository.Save();
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Login", "Account");
        }
    }
}
