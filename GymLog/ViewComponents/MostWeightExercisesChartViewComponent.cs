using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymLog.Models;
using GymLog.Data;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace GymLog.ViewComponents;


    public class MostWeightExercisesChartViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MostWeightExercisesChartViewComponent(AppDbContext context, UserManager<AppUser> userManager)
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
        var weightSumOfExercises = new List<(int,float?)>();
        
        foreach(var workout in workouts)
        {
            foreach(var segment in workout.WorkoutSegments)
            {
                
                foreach (var set in segment.Sets)
                {
                    var element = weightSumOfExercises.Find(a => a.Item1 == segment.ExerciseId);
                    if (element != default)
                    {
                        int index = weightSumOfExercises.IndexOf(element);
                        weightSumOfExercises[index] = (element.Item1, element.Item2 + set.Weight);
                    }
                    else
                    {
                        weightSumOfExercises.Add((segment.ExerciseId, 0));
                    }
                }
            }  
        }
        weightSumOfExercises = weightSumOfExercises.OrderByDescending(item => item.Item2).ToList();
        int n;
        if(weightSumOfExercises.Count >= 0 && weightSumOfExercises.Count < 5)
        {
            n= weightSumOfExercises.Count;
        }
        else
        {
            n = 5;
        }
        var exercises = await _context.Exercises.ToListAsync();
        var labels = new List<string>();
        var values = new List<float?>();
        for(int i=0;i<5;i++)
        {
            if (i < n)
            {
				var exercise = exercises.FirstOrDefault(a => a.Id == weightSumOfExercises[i].Item1);
				labels.Add(exercise.Name);
				values.Add(weightSumOfExercises[i].Item2);
			}
            else
            {
				labels.Add("");
				values.Add(0);
			}

        }
        var chartDataVM = new ChartDataFloatVM
        {
            Labels = labels.ToArray(),
            Values = values.ToArray(),
        };
        return View(chartDataVM);

    }
}
