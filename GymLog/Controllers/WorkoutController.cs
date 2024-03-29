﻿using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;


namespace GymLog.Controllers
{
    public class WorkoutController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public WorkoutController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return RedirectToAction("Login", "Account");
            var accountsVM = await _userManager.Users.Select(user => new AccountVM
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
            }).ToListAsync();
            var users = await _userManager.Users.ToListAsync();
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                if (role == "user")
                {
                    RedirectToAction("Index", "Home");
                }
                else if (role == "admin")
                {
                    var workouts = _context.Workouts.Include(b=>b.AppUser).ToList();
                    var workoutsVM = new List<WorkoutVM>();
                    
                    foreach (var workout in workouts)
                    {
                        var workoutVM = WorkoutToWorkoutVM(workout, new WorkoutVM());
                        workoutsVM.Add(workoutVM);
                    }
                    foreach(var workoutVM in workoutsVM)
                    {
                        var account = accountsVM.FirstOrDefault(a => a.Id == workoutVM.AppUserId);
                        if(account != null)
                            workoutVM.AppUserEmail = account.Email;
                    }
                    return View(workoutsVM);
                }
            }
            return RedirectToAction("Login", "Account");

        }

        //<-----------------------   Set   ---------------------> 

        public IActionResult AddSet(WorkoutVM workoutVM, [FromQuery(Name = "segment")] int segment)
        {
            if (workoutVM == null) throw new ArgumentNullException();
            if (workoutVM.WorkoutSegmentsVM == null) throw new ArgumentNullException();
            if (workoutVM.WorkoutSegmentsVM[segment] == null) throw new ArgumentNullException();
            workoutVM.WorkoutSegmentsVM[segment].SetsVM ??= new List<SetVM>();
            workoutVM.WorkoutSegmentsVM[segment].SetsVM.Add(new SetVM());
            return View(workoutVM.ActionName, workoutVM);
        }
        public IActionResult RemoveSet(WorkoutVM workoutVM, [FromQuery(Name = "segment")] int segment)
        {
            if (workoutVM == null) throw new ArgumentNullException();
            if (workoutVM.WorkoutSegmentsVM == null) throw new ArgumentNullException();
            if (workoutVM.WorkoutSegmentsVM[segment].SetsVM == null) return View("Error");
            if (workoutVM.WorkoutSegmentsVM[segment].SetsVM.Count>1)
                workoutVM.WorkoutSegmentsVM[segment].SetsVM.RemoveAt(workoutVM.WorkoutSegmentsVM[segment].SetsVM.Count - 1);
            ModelState.Clear();
            return View(workoutVM.ActionName, workoutVM);
        }



        //<-----------------------   Exercise   ---------------------> 

        public IActionResult AddWorkoutSegment(WorkoutVM workoutVM, [FromQuery(Name = "exerciseID")] int exerciseId)
        {
            workoutVM.WorkoutSegmentsVM ??= new List<WorkoutSegmentVM>();
            var segment = new WorkoutSegmentVM()
            {
                ExerciseId = exerciseId,
                SetsVM = new List<SetVM>() { new SetVM() }
            };
            workoutVM.WorkoutSegmentsVM.Add(segment);
            return View(workoutVM.ActionName, workoutVM);
        }
        public IActionResult RemoveWorkoutSegment(WorkoutVM workoutVM, [FromQuery(Name = "segment")] int segment)
        {
            if (workoutVM.WorkoutSegmentsVM == null) return View("Error");
            workoutVM.WorkoutSegmentsVM.RemoveAt(segment);
            ModelState.Clear();
            return View(workoutVM.ActionName, workoutVM);
        }


        //<-----------------------   Workout   ---------------------> 
        public async Task<IActionResult> Execute(int id)
        {
            var template = await Context.Templates.Include(t => t.WorkoutSegments).FirstOrDefaultAsync(t => t.Id == id);
            var templateVM = new TemplateVM() { ActionName = "Create" };
            if (template == null) return View("Error");

            templateVM = TemplateToTemplateVM(template, templateVM);

            var workoutVM = new WorkoutVM()
            {
                Name = templateVM.Name,
                WorkoutSegmentsVM = templateVM.WorkoutSegmentsVM,
                Exercises = templateVM.Exercises,
                TemplateId = id,
                ActionName = templateVM.ActionName,
                StartDate = DateTime.Now,
            };
            return View(workoutVM.ActionName, workoutVM);
        }

        //<-----------------------   Create   ---------------------> 
        public async Task<IActionResult> Create(WorkoutVM? workoutVM)
        {
            workoutVM ??= new WorkoutVM();

            if (workoutVM.StartDate == DateTime.MinValue) workoutVM.StartDate = DateTime.Now;

            var user = await _userManager.GetUserAsync(HttpContext.User);
            workoutVM.Exercises = _context.Exercises.Where(a=>a.AppUserId == user.Id || a.AppUserId == null).ToList();

            workoutVM.WorkoutSegmentsVM ??= new List<WorkoutSegmentVM>();
            workoutVM.ActionName = "create";
            return View(workoutVM);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(WorkoutVM workoutVM)
        {
            if (!ModelState.IsValid) return View("Create", workoutVM);

            var user = await _userManager.GetUserAsync(HttpContext.User);
            Workout workout = new Workout();
            workout.Name = workoutVM.Name;
			workout.AppUserId = user.Id;
			workout.StartDate = workoutVM.StartDate;
			var now = DateTime.Now;
			workout.EndDate = now.AddMilliseconds(-now.Millisecond);
			workout.WorkoutSegments = new List<WorkoutSegment>();
            if (workoutVM.TemplateId != 0) _ = workout.TemplateId == workoutVM.TemplateId;
            

            if (workoutVM.WorkoutSegmentsVM != null)
                foreach (var segment in workoutVM.WorkoutSegmentsVM)
                {
                    List<Set> sets = new List<Set>();
                    if (segment.SetsVM != null)
                    {
                        foreach (var set in segment.SetsVM)
                            if (set.isDone)
                            {
                                sets.Add(new Set()
                                {
                                    Weight = set.Weight,
                                    Reps = set.Reps,
                                    //WorkoutSegmentId = template.Id
                                });
                            }
                        if (sets.Count > 0)
                            workout.WorkoutSegments.Add(new WorkoutSegment
                            {
                                WeightType = segment.WeightType,
                                Description = segment.Description,
                                Sets = sets,
                                ExerciseId = segment.ExerciseId,
                            });
                    }
                }
            _context.Add(workout);
            _context.SaveChanges();

            foreach (var segment in workout.WorkoutSegments)
            {
                foreach (var set in segment.Sets)
                    set.WorkoutSegmentId = segment.Id;
            }
            _context.Update(workout);
            _context.SaveChanges();
			return RedirectToAction("Index", "Home");
		}



        //<-----------------------   Edit   ---------------------> 

        public async Task<IActionResult> Edit(WorkoutVM? workoutVM, int? id)
        {
            if (id != null)
            {
                var workout = await _context.Workouts.Include(t => t.WorkoutSegments).FirstOrDefaultAsync(x => x.Id == id);
                workoutVM ??= new WorkoutVM();
                workoutVM.ActionName = "Edit";
                if (workout == null) return View("Error");
                workoutVM = WorkoutToWorkoutVM(workout, workoutVM);
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            workoutVM.Exercises = _context.Exercises.Where(a => a.AppUserId == user.Id || a.AppUserId == null).ToList();
            return View(workoutVM);
        }
        [HttpPost]
        public async Task<IActionResult> EditPost(WorkoutVM workoutVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", workoutVM);
            }
            var workout = await _context.Workouts.Include(t => t.WorkoutSegments).FirstOrDefaultAsync(i => i.Id == workoutVM.Id);
            if (workout == null) return View("Error");
            if (workoutVM.WorkoutSegmentsVM == null) return View("Error");
            var segments = new List<WorkoutSegment>();
            var segmentsVMIds = new List<int>();
            var segmentsIdToRemove = new List<int>();
            //Removing segments that are not in templateVM
            //Creating list with segments IDs
            foreach (var segmentsVM in workoutVM.WorkoutSegmentsVM)
            {
                segmentsVMIds.Add(segmentsVM.Id);
            }
            //Creating list with segments IDs to remove
            foreach (var segment in workout.WorkoutSegments)
            {
                if (!segmentsVMIds.Contains(segment.Id))
                {
                    segmentsIdToRemove.Add(segment.Id);
                }
            }
            //Removing segments & sets
            foreach (var id in segmentsIdToRemove)
            {
                var segmentToRemove = await _context.WorkoutSegments.Include(s => s.Sets).FirstOrDefaultAsync(i => i.Id == id);
                //Removing sets
                if (segmentToRemove != null)
                {
                    if (segmentToRemove.Sets != null)
                    {
                        _context.Sets.RemoveRange(segmentToRemove.Sets);
                        await _context.SaveChangesAsync();
                    }
                    //Removing segments
                    _context.WorkoutSegments.Remove(segmentToRemove);
                    await _context.SaveChangesAsync();
                }
            }

            //Updating remaining segments
            foreach (var segmentVM in workoutVM.WorkoutSegmentsVM)
            {
                var segment = new WorkoutSegment();
                if (segmentVM.Id != 0)
                {
                    segment = await _context.WorkoutSegments.FirstOrDefaultAsync(i => i.Id == segmentVM.Id);
                    if (segment != null)
                    {
                        segment.ExerciseId = segmentVM.ExerciseId;
                        segment.Description = segmentVM.Description;
                        segment.WeightType = segmentVM.WeightType;
                        segment.WorkoutId = workoutVM.Id;
                        _context.WorkoutSegments.Update(segment);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    segment = new WorkoutSegment();
                    segment.ExerciseId = segmentVM.ExerciseId;
                    segment.Description = segmentVM.Description;
                    segment.WeightType = segmentVM.WeightType;
                    segment.WorkoutId = workoutVM.Id;
                    _context.WorkoutSegments.Add(segment);
                    await _context.SaveChangesAsync();

                }
                //Removing sets that are not in segmentVM
                var setsVMIds = new List<int>();
                var setsIdToRemove = new List<int>();
                //Creating list with sets IDs from VM
                if (segmentVM.SetsVM != null)
                    foreach (var setVM in segmentVM.SetsVM)
                    {
                        setsVMIds.Add(setVM.Id);
                    }
                ////Creating list with sets ID to remove
                var tempSegment = await _context.WorkoutSegments.Include(s => s.Sets)
                                    .FirstOrDefaultAsync(i => i.Id == segmentVM.Id);
                if (tempSegment != null && tempSegment.Sets != null)
                    foreach (var set in tempSegment.Sets)
                    {
                        if (!setsVMIds.Contains(set.Id))
                            setsIdToRemove.Add(set.Id);
                    }
                foreach (var id in setsIdToRemove)
                {
                    var setToRemove = _context.Sets.FirstOrDefault(i => i.Id == id);
                    if (setToRemove != null)
                    {
                        _context.Sets.Remove(setToRemove);
                        await _context.SaveChangesAsync();
                    }
                }


                var sets = new List<Set>();
                if (segmentVM.SetsVM != null)
                {
                    foreach (var setVM in segmentVM.SetsVM)
                    {
                        var set = new Set();
                        if (setVM.Id != 0)
                        {
                            set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == setVM.Id);

                            if (set != null)
                            {
                                set.Reps = setVM.Reps;
                                set.Weight = setVM.Weight;
                                set.WorkoutSegmentId = segment.Id;
                                _context.Update(set);
                                await _context.SaveChangesAsync();

                                sets.Add(set);
                            }
                        }
                        else
                        {
                            set.Reps = setVM.Reps;
                            set.Weight = setVM.Weight;
                            set.WorkoutSegmentId = segment.Id;
                            _context.Sets.Add(set);
                            await _context.SaveChangesAsync();

                            sets.Add(set);
                        }
                    }
                }
                segment.Sets = sets;
                segments.Add(segment);
            }
            workout.Name = workoutVM.Name;
            workout.WorkoutSegments = segments;
            workout.StartDate = workoutVM.StartDate;
            workout.EndDate = workoutVM.EndDate;
            _context.Update(workout);
            await _context.SaveChangesAsync();

			/*
            _context.Sets.Where(s => s.WorkoutSegmentId == null);*/
			return RedirectToAction("History", "Home");
		}
		public async Task<IActionResult> Delete(int id)
		{
			var workout = await _context.Workouts.FirstOrDefaultAsync(s => s.Id == id);
			if (workout != null)
			{
				_context.Workouts.Remove(workout);
				var segments = _context.WorkoutSegments.Where(x => x.WorkoutId == id).ToList();
				if (segments.Any()) _context.RemoveRange(segments);
				foreach (var segment in segments)
				{
					var sets = _context.Sets.Where(x => x.WorkoutSegmentId == id).ToList();
					if (sets.Any()) _context.RemoveRange(sets);
				}
				_context.SaveChanges();
			}
			return RedirectToAction("History", "Home");
		}
	}
}
