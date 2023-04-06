using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

		[HttpPost]
		public IActionResult Create(Set set)
		{
			if(!ModelState.IsValid)
			{
				return View(set);
			}
			_context.Sets.Add(set);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int id)
		{
			var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == id);
			if(set == null) return View("Error");
			var setVM = new SetVM()
			{
				Id = id,
				Weight = set.Weight,
				Reps = set.Reps,
				Description = set.Description,
				ExcerciseSetId = set.ExcerciseSetId,
			};
			return View(setVM);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(SetVM setVM)
		{
			if(!ModelState.IsValid) 
			{
				ModelState.AddModelError("", "Failed to edit Set");
				return View(setVM);
			}
			var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == setVM.Id);

			if(set != null)
			{
				set.Weight = setVM.Weight;
				set.Reps = setVM.Reps;
				set.Description = setVM.Description;
				set.ExcerciseSetId = setVM.ExcerciseSetId;
				_context.Update(set);
				_context.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == id);
			if (set == null) return View("Error");
			return View(set);
		}
		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteSet(Set Set)
		{
			_context.Remove(Set);
			_context.SaveChanges();
			return RedirectToAction("Index");		}
	}
}
