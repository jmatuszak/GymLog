using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymLog.Models;
using GymLog.Data;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GymLog.ViewComponents;


    public class FavouriteExercisesChartViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public FavouriteExercisesChartViewComponent(AppDbContext context, UserManager<AppUser> userManager)
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
        var exercisesExecutes = new List<(int,int)>();
        
        foreach(var workout in workouts)
        {
            foreach(var segment in workout.WorkoutSegments)
            {
                var element = exercisesExecutes.Find(a => a.Item1 == segment.ExerciseId);
                if(element != default)
                {
                    int index = exercisesExecutes.IndexOf(element);
                    exercisesExecutes[index] = (element.Item1,element.Item2+1);
                }
                else
                {
                    exercisesExecutes.Add((segment.ExerciseId, 1));
                }
            }  
        }
        exercisesExecutes = exercisesExecutes.OrderByDescending(item => item.Item2).ToList();
        int n;
        if(exercisesExecutes.Count >= 0 && exercisesExecutes.Count < 5)
        {
            n= exercisesExecutes.Count;
        }
        else
        {
            n = 5;
        }
        var exercises = await _context.Exercises.ToListAsync();
        var labels = new List<string>();
        var values = new List<int>();
        for(int i=0;i<5;i++)
        {
            if (i < n)
            {
				var exercise = exercises.FirstOrDefault(a => a.Id == exercisesExecutes[i].Item1);
				labels.Add(exercise.Name);
				values.Add(exercisesExecutes[i].Item2);
			}
            else
            {
				labels.Add("");
				values.Add(0);
			}

        }
        var chartDataVM = new ChartDataVM
        {
            Labels = labels.ToArray(),
            Values = values.ToArray(),
        };
        return View(chartDataVM);

    }
}
