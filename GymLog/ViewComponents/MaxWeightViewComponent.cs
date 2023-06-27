using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymLog.Models;
using GymLog.Data;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GymLog.ViewComponents;


public class MaxWeightViewComponent : ViewComponent
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public MaxWeightViewComponent(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var exercises = await _context.Exercises.Include(x => x.BodyParts).ToListAsync();
        var exercisesVM = ExercisesToExercisesVM(exercises);
        
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
            });
        }
        return exercisesVM;
    }
}
