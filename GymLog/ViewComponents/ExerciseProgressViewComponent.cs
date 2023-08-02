using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymLog.Models;
using GymLog.Data;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace GymLog.ViewComponents;


public class ExerciseProgressViewComponent : ViewComponent
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public ExerciseProgressViewComponent(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var workouts = await _context.Workouts.Include(x => x.WorkoutSegments)
            .ThenInclude(segment => segment.Sets)
            .Where(x => x.AppUserId == user.Id).ToListAsync();
        var allExercises = await _context.Exercises.Include(x => x.BodyParts).ToListAsync();
        var exercisesIds = new List<int>();
        //Adding to the list Ids of exercises that where exercised by user
        foreach (var workout in workouts)
        {
            if (workout.WorkoutSegments != null)
                foreach (var segment in workout.WorkoutSegments)
                {
                    if (segment.Sets != null)
                    {
                        if (!exercisesIds.Contains(segment.ExerciseId))
                            exercisesIds.Add(segment.ExerciseId);
                    }
                }
        }
        var userExercises = new List<Exercise>();
        foreach (var id in exercisesIds)
        {
            var exercise = allExercises.FirstOrDefault(a => a.Id == id);
            if (exercise != null)
                userExercises.Add(exercise);
        }

        var exercisesVM = ExercisesToExercisesVM(userExercises);

        return View(exercisesVM);
    }
    private List<ExerciseVM> ExercisesToExercisesVM(List<Exercise> exercises)
    {
        var exercisesVM = new List<ExerciseVM>();
        foreach (var exercise in exercises)
        {
            var bodyPartsVM = new List<BodyPartVM>();
            if (exercise.BodyParts != null)
            {
                foreach (var item in exercise.BodyParts)
                {
                    bodyPartsVM.Add(new()
                    {
                        Id = item.Id,
                        Name = item.Name,
                    });
                }
            }
            exercisesVM.Add(new ExerciseVM()
            {
                Id = exercise.Id,
                Name = exercise.Name,
                BodyPartsVM = bodyPartsVM,
                ImageSrc = exercise.ImageSrc,
            });
        }
        return exercisesVM;
    }
}