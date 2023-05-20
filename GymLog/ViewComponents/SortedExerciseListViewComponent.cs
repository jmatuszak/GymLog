using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymLog.Models;
using GymLog.Data;
using GymLog.ViewModels;

namespace GymLog.ViewComponents;


public class SortedExerciseListViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public SortedExerciseListViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var exercises = await _context.Exercises.Include(x => x.BodyParts).ToListAsync();
        var bodyParts = await _context.BodyParts.ToListAsync();
        var sortedExercisesVM = new List<SortedExerciseVM>();
        var exercisesVM = ExercisesToExercisesVM(exercises);
        foreach(var bodyPart in bodyParts)
        {
            var filteredExercisesVM = exercisesVM.Where(a => a.BodyPartsVM.Any(b => b.Id == bodyPart.Id)).ToList();
            sortedExercisesVM.Add(new SortedExerciseVM()
            {
                Name = bodyPart.Name,
                ExercisesVM = filteredExercisesVM
            });
        }

        return View(sortedExercisesVM);
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
