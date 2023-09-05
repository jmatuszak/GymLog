using GymLog.Data;
using GymLog.Data.Enum;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace GymLog.Controllers
{
    public class TemplateController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TemplateController(AppDbContext context, UserManager<AppUser> userManager)
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

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                if (role == "user")
                {
                    RedirectToAction("Index", "Home");
                }
                else if (role == "admin")
                {
                    var templates = _context.Templates.Include(s => s.WorkoutSegments).ToList();
                    List<TemplateVM> templatesVM = new List<TemplateVM>();
                    foreach (var template in templates)
                    {
                        var segmentsVM = new List<WorkoutSegmentVM>();
                        if (template.WorkoutSegments != null)
                            foreach (var segment in template.WorkoutSegments)
                            {
                                var segmentWithSets = await _context.WorkoutSegments.Include(s => s.Sets)
                                    .FirstOrDefaultAsync(s => s.Id == segment.Id);
                                var setsVM = new List<SetVM>();
                                if (segmentWithSets != null && segmentWithSets.Sets != null)
                                {
                                    foreach (var set in segmentWithSets.Sets)
                                    {
                                        setsVM.Add(new SetVM()
                                        {
                                            Weight = set.Weight,
                                            Reps = set.Reps,
                                            WorkoutSegmentId = set.WorkoutSegmentId,
                                        });
                                    }
                                }

                                segmentsVM.Add(new WorkoutSegmentVM
                                {
                                    WeightType = segment.WeightType,
                                    Description = segment.Description,
                                    SetsVM = setsVM,
                                    ExerciseId = segment.ExerciseId,
                                });
                            }
                        var templateVM = new TemplateVM()
                        {
                            Id = template.Id,
                            Name = template.Name,
                            WorkoutSegmentsVM = segmentsVM,
                            AppUserId = template.AppUserId,
                        };
                        templatesVM.Add(templateVM);
                    }
                    foreach (var templateVM in templatesVM)
                    {
                        var account = accountsVM.FirstOrDefault(a => a.Id == templateVM.AppUserId);
                        if (account != null)
                            templateVM.AppUserEmail = account.Email;
                    }
                    return View(templatesVM);
                }
            }
            return RedirectToAction("Login", "Account");
        }



        //<-----------------------   Set   ---------------------> 

        public IActionResult AddSet(TemplateVM templateVM, [FromQuery(Name = "segment")] int segment)
        {
            if (templateVM == null) throw new ArgumentNullException();
            if (templateVM.WorkoutSegmentsVM == null) throw new ArgumentNullException();
            if (templateVM.WorkoutSegmentsVM[segment] == null) throw new ArgumentNullException();
            templateVM.WorkoutSegmentsVM[segment].SetsVM ??= new List<SetVM>();
            templateVM.WorkoutSegmentsVM[segment].SetsVM.Add(new SetVM());
            return View(templateVM.ActionName, templateVM);
        }
        public IActionResult RemoveSet(TemplateVM templateVM, [FromQuery(Name = "segment")] int segment)
        {
            if (templateVM == null) throw new ArgumentNullException();
            if (templateVM.WorkoutSegmentsVM == null) throw new ArgumentNullException();
            if (templateVM.WorkoutSegmentsVM[segment].SetsVM == null) return View("Error");
            if (templateVM.WorkoutSegmentsVM[segment].SetsVM.Count>1)
                templateVM.WorkoutSegmentsVM[segment].SetsVM.RemoveAt(templateVM.WorkoutSegmentsVM[segment].SetsVM.Count - 1);
            ModelState.Clear();
            return View(templateVM.ActionName, templateVM);
        }



        //<-----------------------   Exercise   ---------------------> 

        public IActionResult AddWorkoutSegment(TemplateVM templateVM, [FromQuery(Name = "exerciseID")] int exerciseId)
        {
            templateVM.WorkoutSegmentsVM ??= new List<WorkoutSegmentVM>();
            var segment = new WorkoutSegmentVM()
            {
                ExerciseId = exerciseId,
                SetsVM = new List<SetVM>() { new SetVM() }
            };
            templateVM.WorkoutSegmentsVM.Add(segment);
            return View(templateVM.ActionName, templateVM);

        }
        public IActionResult RemoveWorkoutSegment(TemplateVM templateVM, [FromQuery(Name = "segment")] int segment)
        {
            if (templateVM.WorkoutSegmentsVM == null) return View("Error");
            templateVM.WorkoutSegmentsVM.RemoveAt(segment);
            ModelState.Clear();
            return View(templateVM.ActionName, templateVM);
        }


        //<-----------------------   Create   ---------------------> 
        public async Task<IActionResult> Create(TemplateVM? templateVM)
        {
            templateVM ??= new TemplateVM();

            var user = await _userManager.GetUserAsync(HttpContext.User);
            templateVM.Exercises = _context.Exercises.Where(a => a.AppUserId == user.Id || a.AppUserId == null).ToList();

            templateVM.WorkoutSegmentsVM ??= new List<WorkoutSegmentVM>();
            templateVM.ActionName = "create";
            return View(templateVM);

        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(TemplateVM templateVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", templateVM);
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var template = new Template()
            {
                Name = templateVM.Name,
                WorkoutSegments = new List<WorkoutSegment>(),
                AppUserId = user.Id,
            };

            if (templateVM.WorkoutSegmentsVM != null)
                foreach (var segment in templateVM.WorkoutSegmentsVM)
                {
                    List<Set> sets = new List<Set>();
                    if (segment.SetsVM != null)
                        foreach (var set in segment.SetsVM)
                            sets.Add(new Set()
                            {
                                Weight = set.Weight,
                                Reps = set.Reps,
                                //WorkoutSegmentId = template.Id
                            });
                    template.WorkoutSegments.Add(new WorkoutSegment
                    {
                        WeightType = segment.WeightType,
                        Description = segment.Description,
                        //Order = segment.Order,
                        Sets = sets,
                        ExerciseId = segment.ExerciseId,
                    });
                }
            _context.Add(template);
            _context.SaveChanges();

            foreach (var segment in template.WorkoutSegments)
            {
                foreach (var set in segment.Sets)
                    set.WorkoutSegmentId = segment.Id;
            }
            _context.Update(template);
            _context.SaveChanges();
			return RedirectToAction("Index", "Home");
		}



        //<-----------------------   Edit   ---------------------> 

        public async Task<IActionResult> Edit(TemplateVM? templateVM, int? id)
        {
            if (id != null)
            {
                var template = await _context.Templates.Include(t => t.WorkoutSegments).FirstOrDefaultAsync(t => t.Id == id);
                templateVM ??= new TemplateVM();
                templateVM.ActionName = "Edit";
                if (template == null) return View("Error");
                templateVM = TemplateToTemplateVM(template, templateVM);
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            templateVM.Exercises = _context.Exercises.Where(a => a.AppUserId == user.Id || a.AppUserId == null).ToList();

            return View(templateVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(TemplateVM templateVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", templateVM);
            }
            var template = await _context.Templates.Include(t => t.WorkoutSegments).FirstOrDefaultAsync(i => i.Id == templateVM.Id);
            if (template == null) return View("Error");
            if (templateVM.WorkoutSegmentsVM == null) return View("Error");
            var segments = new List<WorkoutSegment>();
            var segmentsVMIds = new List<int>();
            var segmentsIdToRemove = new List<int>();
            //Removing segments that are not in templateVM
            //Creating list with segments IDs
            foreach (var segmentsVM in templateVM.WorkoutSegmentsVM)
            {
                segmentsVMIds.Add(segmentsVM.Id);
            }
            //Creating list with segments IDs to remove
            foreach (var segment in template.WorkoutSegments)
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
            foreach (var segmentVM in templateVM.WorkoutSegmentsVM)
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
                        segment.TemplateId = templateVM.Id;
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
                    segment.TemplateId = templateVM.Id;
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
            template.Name = templateVM.Name;
            template.WorkoutSegments = segments;
            _context.Update(template);
            await _context.SaveChangesAsync();

			/*
            _context.Sets.Where(s => s.WorkoutSegmentId == null);*/
			return RedirectToAction("Index", "Home");
		}


        public async Task<IActionResult> Delete(int id)
        {
            var template = await _context.Templates.FirstOrDefaultAsync(s => s.Id == id);
            if (template != null)
            {
                var workouts = await _context.Workouts.Where(a => a.TemplateId == template.Id).ToListAsync();
                foreach(var workout in workouts)
                {
                    workout.TemplateId = null;
                }
                _context.UpdateRange(workouts);
                _context.SaveChanges();
                _context.Templates.Remove(template);
                var segments = _context.WorkoutSegments.Where(x => x.TemplateId == id).ToList();
                if (segments.Any()) _context.RemoveRange(segments);
                foreach (var segment in segments)
                {
                    var sets = _context.Sets.Where(x => x.WorkoutSegmentId == id).ToList();
                    if (sets.Any()) _context.RemoveRange(sets);
                }
                _context.SaveChanges();
            }
			return RedirectToAction("Index", "Home");
		}
    }
}