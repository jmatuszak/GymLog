using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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

        //<-----------------------   Set   ---------------------> 
        public TemplateVM AddSet(TemplateVM templateVM, int i)
        {
            if (templateVM == null) throw new ArgumentNullException();
            if (templateVM.ExcerciseSetsVM == null) throw new ArgumentNullException();
            if (templateVM.ExcerciseSetsVM[i].SetsVM == null) 
                templateVM.ExcerciseSetsVM[i].SetsVM = new List<SetVM>();
            templateVM.ExcerciseSetsVM[i].SetsVM.Add(new SetVM());
            return templateVM;

        }
        public TemplateVM RemoveSet(TemplateVM templateVM, int element, int set)
        {
            if (templateVM == null) throw new ArgumentNullException();
            if (templateVM.ExcerciseSetsVM == null) throw new ArgumentNullException();
            if (templateVM.ExcerciseSetsVM[element].SetsVM == null)
                templateVM.ExcerciseSetsVM[element].SetsVM = new List<SetVM>();
            var delete = templateVM.ExcerciseSetsVM[element].SetsVM[set];
            templateVM.ExcerciseSetsVM[element].SetsVM.RemoveAt(set);
            return templateVM;
        }


        public IActionResult AddSetCreate(TemplateVM templateVM, [FromQuery(Name = "set")] int i)
        {
            templateVM = AddSet(templateVM, i);
            return View("Create", templateVM);
        }
        public IActionResult RemoveSetCreate(TemplateVM templateVM, [FromQuery(Name = "element")] int i, [FromQuery(Name = "set")] int j)
        {
            templateVM = RemoveSet(templateVM, i,j);
            ModelState.Clear();
            return View("Create", templateVM);
        }



        //<-----------------------   Excercise   ---------------------> 

        public TemplateVM AddExcerciseSet(TemplateVM templateVM)
        {
            if (templateVM.ExcerciseSetsVM == null) templateVM.ExcerciseSetsVM = new List<ExcerciseSetVM>();
            templateVM.ExcerciseSetsVM.Add(new ExcerciseSetVM());
            return templateVM;
        }
        public TemplateVM RemoveExcerciseSet(TemplateVM templateVM, int element)
        {
            if (templateVM.ExcerciseSetsVM == null) templateVM.ExcerciseSetsVM = new List<ExcerciseSetVM>();
            templateVM.ExcerciseSetsVM.RemoveAt(element);
            ModelState.Clear();
            return templateVM;
        }

        public IActionResult AddExcerciseSetCreate(TemplateVM templateVM)
        {
            if (templateVM.ExcerciseSetsVM == null) templateVM.ExcerciseSetsVM = new List<ExcerciseSetVM>();
            templateVM = AddExcerciseSet(templateVM);
            return View("Create", templateVM);

        }
        public IActionResult RemoveExcerciseSetCreate(TemplateVM templateVM, [FromQuery(Name = "element")] int element)
        {
            if (templateVM.ExcerciseSetsVM == null) return View("Error");
            templateVM = RemoveExcerciseSet(templateVM,element);
            return View("Create", templateVM);
        }


        //<-----------------------   Create   ---------------------> 
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
