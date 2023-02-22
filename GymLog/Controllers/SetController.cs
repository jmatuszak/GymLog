using GymLog.Data;
using Microsoft.AspNetCore.Mvc;

namespace GymLog.Controllers
{
	public class SetController : Controller
	{
		private readonly AppDbContext _context;

		public SetController(AppDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var sets = _context.Sets.ToList();
			return View(sets);
		}

		public IActionResult Create()
		{

			return View();
		}
	}
}
