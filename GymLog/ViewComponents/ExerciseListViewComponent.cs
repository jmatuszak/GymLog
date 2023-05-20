using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymLog.Models;
using GymLog.Data;
using GymLog.ViewModels;

namespace GymLog.ViewComponents;


public class ExerciseListViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public ExerciseListViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? searchString)
    {
        var exercises = await _context.Exercises.Include(x => x.BodyParts).ToListAsync();
        var exercisesVM = ExercisesToExercisesVM(exercises);
        if (searchString == null) return View(exercisesVM);
        
        var searchedExercisesVM = exercisesVM.Where(a => a.Name.Contains(searchString)).ToList();


        return View(searchedExercisesVM);
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
