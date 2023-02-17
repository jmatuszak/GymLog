using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            List<SelectListItem> selectListItems = _context.BodyParts.
    Select(a => new SelectListItem
    {
        Text = a.Name,
        Value = a.Id.ToString(),
    }).ToList();
            ViewBag.BodyPartId = selectListItems;
            return View();
        }
        public IActionResult Create()
        {
            var bodyParts = _context.BodyParts;
            var bodyPartListVM = new List<BodyPartVM>();
            foreach(var item in bodyParts)
            {
                bodyPartListVM.Add(new BodyPartVM
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }
            ViewBag.BodyParts = bodyPartListVM;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Excercise excercise, List<int> BodyPartIdList)
        {
            if (!ModelState.IsValid)
            {
                return View(excercise);
            }
            _context.Excercises.Add(excercise);
            _context.SaveChanges();

            foreach (var item in BodyPartIdList)
            {
                BodyPartExcercise bodyPartExcercise = new BodyPartExcercise()
                {
                    BodyPartId = item,
                    ExcerciseId = excercise.Id,
                    BodyPart = _context.BodyParts.Where(x => x.Id == item).FirstOrDefault(),
                    Excercise = excercise
                };
                _context.BodyPartExcercises.Add(bodyPartExcercise);
            }
            return RedirectToAction("Index");
        }
    }
}
