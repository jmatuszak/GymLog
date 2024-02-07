using GymLog.Data;
using GymLog.Data.Enum;
using GymLog.Interfaces;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace GymLog.Controllers
{
	public class WorkoutSegmentController : Controller
	{
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWorkoutSegmentRepository _workoutSegmentRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ISetRepository _setRepository;

        public WorkoutSegmentController(AppDbContext context, UserManager<AppUser> userManager, IWorkoutSegmentRepository workoutSegmentRepository, IExerciseRepository exerciseRepository, ISetRepository setRepository)
        {
            _context = context;
            _userManager = userManager;
            _workoutSegmentRepository = workoutSegmentRepository;
            _exerciseRepository = exerciseRepository;
            _setRepository = setRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                var list = await _workoutSegmentRepository.GetListAsync();
                return View(list);
            }
            else return RedirectToAction("Login", "Account");
        }

        public IActionResult AddSet(WorkoutSegmentVM workoutSegmentVM)
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                workoutSegmentVM.SetsVM ??= new List<SetVM>();
                workoutSegmentVM.SetsVM.Add(new SetVM());
                return View(workoutSegmentVM.ActionName, workoutSegmentVM);
            }
            else return RedirectToAction("Login", "Account");
        }
        
        public IActionResult RemoveSet(WorkoutSegmentVM workoutSegmentVM)
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                workoutSegmentVM.SetsVM ??= new List<SetVM>();
                workoutSegmentVM.SetsVM.RemoveAt(workoutSegmentVM.SetsVM.Count - 1);
                return View(workoutSegmentVM.ActionName, workoutSegmentVM);
            }
            else return RedirectToAction("Login", "Account");
        }
        

        //<-----------------------  CREATE   --------------------->
        public async Task<IActionResult> Create(WorkoutSegmentVM? workoutSegmentVM)
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                workoutSegmentVM ??= new WorkoutSegmentVM();
                workoutSegmentVM.SetsVM ??= new List<SetVM>();
                workoutSegmentVM.SetsVM.Add(new SetVM());
                workoutSegmentVM.ActionName = "Create";
                workoutSegmentVM.Exercises = await _exerciseRepository.GetListAsync() as List<Exercise>;
                return View(workoutSegmentVM);
            }
            else return RedirectToAction("Login", "Account");
        }

        [HttpPost]
		public IActionResult CreatePost(WorkoutSegmentVM workoutSegmentVM)
		{
            if (!ModelState.IsValid) return View("Create", workoutSegmentVM);

            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                var sets = new List<Set>();
                if (workoutSegmentVM.SetsVM != null)
                    foreach (var item in workoutSegmentVM.SetsVM)
                    {
                        var set = new Set()
                        {
                            Weight = item.Weight,
                            Reps = item.Reps,
                            WorkoutSegmentId = item.WorkoutSegmentId
                        };
                        sets.Add(set);
                    }
                var workoutSegment = new WorkoutSegment()
                {
                    Description = workoutSegmentVM.Description,
                    ExerciseId = workoutSegmentVM.ExerciseId,
                    TemplateId = workoutSegmentVM.TemplateId,
                    Sets = sets,
                    WeightType = workoutSegmentVM.WeightType,
                };
                _workoutSegmentRepository.Insert(workoutSegment);
                _workoutSegmentRepository.Save();
                return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Account");
		}



        //<-----------------------  UPDATE   --------------------->   


        public async Task<IActionResult> Edit(int id)
		{
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                var exercises = await _exerciseRepository.GetListAsync() as List<Exercise>;

                var workoutSegment = await _workoutSegmentRepository.GetByIdAsync(id);
                var setsVM = new List<SetVM>();
                foreach (var item in workoutSegment.Sets)
                {
                    SetVM setVM = new SetVM()
                    {
                        Id = item.Id,
                        Weight = item.Weight,
                        Reps = item.Reps,
                        WorkoutSegmentId = item.WorkoutSegmentId,
                    };
                    setsVM.Add(setVM);
                }
                var WorkoutSegmentVM = new WorkoutSegmentVM()
                {
                    Id = workoutSegment.Id,
                    Description = workoutSegment.Description,
                    TemplateId = workoutSegment.TemplateId,
                    ExerciseId = workoutSegment.ExerciseId,
                    Exercises = exercises,
                    SetsVM = setsVM
                };

                WorkoutSegmentVM.ActionName = "Edit";
                return View(WorkoutSegmentVM);
            }
            else return RedirectToAction("Login", "Account");
		}

        [HttpPost]
        public async Task<IActionResult> EditPost(WorkoutSegmentVM workoutSegmentVM)
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                if (!ModelState.IsValid) return View("Edit", workoutSegmentVM);

                var oldSets = _context.Sets.Where(x => x.WorkoutSegmentId == workoutSegmentVM.Id).ToList();
                _context.Sets.RemoveRange(oldSets);

                var workoutSegment = await _workoutSegmentRepository.GetByIdAsync(workoutSegmentVM.Id);
                var sets = new List<Set>();
                foreach (var item in workoutSegmentVM.SetsVM)
                {
                    Set set = new Set()
                    {
                        Weight = item.Weight,
                        Reps = item.Reps,
                        WorkoutSegmentId = item.WorkoutSegmentId,
                    };
                    sets.Add(set);
                }
                if (workoutSegment != null)
                {
                    workoutSegment.Id = workoutSegmentVM.Id;
                    workoutSegment.Description = workoutSegmentVM.Description;
                    workoutSegment.ExerciseId = workoutSegmentVM.ExerciseId;
                    workoutSegment.TemplateId = workoutSegmentVM.TemplateId;
                    workoutSegment.Sets = sets;
                    _workoutSegmentRepository.Update(workoutSegment);
                    _workoutSegmentRepository.Save();
                }
                return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Account");
		}

        //<-----------------------  DELETE   ---------------------> 

        public async Task<IActionResult> Delete(int id)
        {
            if (User.IsInRole("user")) return RedirectToAction("Index", "Home");
            else if (User.IsInRole("admin"))
            {
                var WorkoutSegment = await _workoutSegmentRepository.GetByIdAsync(id);
                if (WorkoutSegment != null)
                {
                    _context.WorkoutSegments.Remove(WorkoutSegment);
                    var sets = await _setRepository.GetSetWithWorkoutTemplateIdListAsync(id);
                    _setRepository.DeleteRange(sets);
                    if (sets.Any()) _context.RemoveRange(sets);
                    _workoutSegmentRepository.Save();
                }
                return RedirectToAction("History", "Home");
            }
            else return RedirectToAction("Login", "Account");
		}

    }
}
