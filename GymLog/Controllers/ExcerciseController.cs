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
        public async Task<IActionResult> DeleteBodyPartPart(int id)
        {
            var excercise = await _context.Excercises.FirstOrDefaultAsync(i => i.Id == id);
            if (excercise == null) return View("Error");
            _context.Remove(excercise);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
