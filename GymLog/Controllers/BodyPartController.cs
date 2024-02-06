using GymLog.Data;
using GymLog.Interfaces;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Controllers
{
    public class BodyPartController : BaseController
    {
        private readonly IBodyPartRepository _repo;

        public BodyPartController(IBodyPartRepository bodyPartRepository)
        {
            _repo = bodyPartRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            if (User.IsInRole("admin"))
            {
                var bodyParts = await _repo.GetBodyPartListAsync();
                return View(bodyParts);
            }
            else return RedirectToAction("Login", "Account");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BodyPart bodyPart)
        {
            if(!ModelState.IsValid) return View(bodyPart);

            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            if (User.IsInRole("admin"))
            {
                _repo.InsertBodyPart(bodyPart);
                _repo.Save();

                return RedirectToAction("Index");
            }
            else return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            if (User.IsInRole("admin"))
            {
                var bodyPart = await _repo.GetBodyPartByIdAsync(id);
                if (bodyPart == null) return View("Error");

                var bodyPartVM = BodyPartToBodyPartVM(bodyPart);
                return View(bodyPartVM);
            }
            else return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,BodyPartVM bodyPartVM)
        {

            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            if (User.IsInRole("admin"))
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Failed to edit BodyPart");
                    return View("Edit", bodyPartVM);
                }
                var bodyPart = await _repo.GetBodyPartByIdAsync(id);

                if (bodyPart != null)
                {
                    bodyPart.Id = id;
                    bodyPart.Name = bodyPartVM.Name;
                    _repo.UpdateBodyPart(bodyPart);
                    _repo.Save();
                }
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Login", "Account");
        }


		[HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteBodyPartPart(int id)
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            if (User.IsInRole("admin"))
            {
                var bodyPart = await _repo.GetBodyPartByIdAsync(id);
                if (bodyPart == null) return View("Error");

                
                _repo.DeleteBodyPart(bodyPart);
                _repo.Save();
                return RedirectToAction("Index", "BodyPart");
            }
            else return RedirectToAction("Login", "Account");
        }


	}
}
