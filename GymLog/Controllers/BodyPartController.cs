using GymLog.Data;
using GymLog.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymLog.Controllers
{
    public class BodyPartController : Controller
    {
        private readonly IBodyPartRepository _bodyPartRepository;

        public BodyPartController(IBodyPartRepository bodyPartRepository)
        {
            _bodyPartRepository = bodyPartRepository;
        }
        public async Task<IActionResult> Index()
        {
            var bodyParts = await _bodyPartRepository.GetAll();
            return View(bodyParts);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var bodyPart = await _bodyPartRepository.GetById(id);
            return View(bodyPart);
        }


    }
}
