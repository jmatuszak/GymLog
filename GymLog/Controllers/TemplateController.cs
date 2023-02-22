using GymLog.Data;
using GymLog.Models;
using Microsoft.AspNetCore.Mvc;

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
		public IActionResult Create()
		{

			return View();
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
