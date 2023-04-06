using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Controllers
{
	public class TemplateController : Controller
	{
		private readonly AppDbContext _context;

		public TemplateController(AppDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var templates =  _context.Templates.ToList();
			return View(templates);
		}





        public async Task<List<CreateExcerciseConcatVM>> CreateExcerciseConcatList()
        {
            var excercises = await _context.Excercises.ToListAsync();
            var excercisesVM = new List<CreateExcerciseConcatVM>();

            foreach (var item in excercises)
            {
                excercisesVM.Add(new CreateExcerciseConcatVM()
                {
                    Id = item.Id,
                    Name = item.Name + " - " + item.WeightType,
                });
            }
            return excercisesVM;
        }
        public ExcerciseSetVM AddSet(ExcerciseSetVM ExcerciseSetVM)
        {
            if (ExcerciseSetVM.SetsVM == null) ExcerciseSetVM.SetsVM = new List<SetVM>();
            ExcerciseSetVM.SetsVM.Add(new SetVM());
            return ExcerciseSetVM;

        }
        public ExcerciseSetVM RemoveSet(ExcerciseSetVM ExcerciseSetVM)
        {
            if (ExcerciseSetVM.SetsVM.Count < 1) return ExcerciseSetVM;
            ExcerciseSetVM.SetsVM.RemoveAt(ExcerciseSetVM.SetsVM.Count - 1);
            return ExcerciseSetVM;
        }

        public TemplateVM AddExcerciseSet(TemplateVM templateVM)
        {
            if (templateVM.ExcerciseSetsVM == null) templateVM.ExcerciseSetsVM = new List<ExcerciseSetVM>();
            templateVM.ExcerciseSetsVM.Add(new ExcerciseSetVM());
            return templateVM;

        }



        //<-----------------------  CREATE   --------------------->   


        public IActionResult AddExcerciseSetCreate(TemplateVM templateVM)
        {
            if (templateVM.ExcerciseSetsVM == null) templateVM.ExcerciseSetsVM = new List<ExcerciseSetVM>();
            templateVM = AddExcerciseSet(templateVM);
            return View("Create", templateVM);

        }
        public IActionResult RemoveExcerciseSetCreate(ExcerciseSetVM ExcerciseSetVM)
        {
            if (ExcerciseSetVM.SetsVM == null) return View("Error");
            ExcerciseSetVM = RemoveSet(ExcerciseSetVM);
            return View("Create", ExcerciseSetVM);
        }

        public async Task<IActionResult> Create(TemplateVM? templateVM)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            if (templateVM.ExcercisesConcatVM == null)
            {
                var excercisesConcatVM = await CreateExcerciseConcatList();
                templateVM.ExcercisesConcatVM = excercisesConcatVM;
            }
            templateVM = AddExcerciseSet(templateVM);
            return View(templateVM);
        }
        [HttpPost]
		public IActionResult Create(Template template)
		{
			if(!ModelState.IsValid)
			{
				return View(template);
			}
			_context.Add(template);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}


		
	}
}
