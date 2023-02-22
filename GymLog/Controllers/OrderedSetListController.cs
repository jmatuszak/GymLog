using Microsoft.AspNetCore.Mvc;

namespace GymLog.Controllers
{
	public class OrderedSetListController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
