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
        public SetCollectionVM AddSet(SetCollectionVM setCollectionVM)
        {
            if (setCollectionVM.SetsVM == null) setCollectionVM.SetsVM = new List<SetVM>();
            setCollectionVM.SetsVM.Add(new SetVM());
            return setCollectionVM;

        }
        public SetCollectionVM RemoveSet(SetCollectionVM setCollectionVM)
        {
            if (setCollectionVM.SetsVM.Count < 1) return setCollectionVM;
            setCollectionVM.SetsVM.RemoveAt(setCollectionVM.SetsVM.Count - 1);
            return setCollectionVM;
        }

        public TemplateVM AddSetCollection(TemplateVM templateVM)
        {
            if (templateVM.SetCollectionsVM == null) templateVM.SetCollectionsVM = new List<SetCollectionVM>();
            templateVM.SetCollectionsVM.Add(new SetCollectionVM());
            return templateVM;

        }



        //<-----------------------  CREATE   --------------------->   


        public IActionResult AddSetCollectionCreate(TemplateVM templateVM)
        {
            if (templateVM.SetCollectionsVM == null) templateVM.SetCollectionsVM = new List<SetCollectionVM>();
            templateVM = AddSetCollection(templateVM);
            return View("Create", templateVM);

        }
        public IActionResult RemoveSetCollectionCreate(SetCollectionVM setCollectionVM)
        {
            if (setCollectionVM.SetsVM == null) return View("Error");
            setCollectionVM = RemoveSet(setCollectionVM);
            return View("Create", setCollectionVM);
        }

        public async Task<IActionResult> Create(TemplateVM? templateVM)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            if (templateVM.ExcercisesConcatVM == null)
            {
                var excercisesConcatVM = await CreateExcerciseConcatList();
                templateVM.ExcercisesConcatVM = excercisesConcatVM;
            }
            templateVM = AddSetCollection(templateVM);
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
