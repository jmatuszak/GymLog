using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymLog.Models;
using GymLog.Data;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GymLog.ViewComponents;


public class StatisticsViewComponent : ViewComponent
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public StatisticsViewComponent(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var workouts = await GetWorkouts(user);
        var sets = await GetSets(workouts);
        var repsSum = sets.Sum(s => s.Reps);
        var weightSum = sets.Sum(s => s.Weight);
        repsSum ??= 0;
        weightSum ??= 0;
        var seconds = (int)(workouts.Sum(x=>(x.EndDate - x.StartDate).TotalSeconds));
        var duration = new TimeSpan(0, 0, seconds);


        var statisticsVM = new StatisticsVM()
        {
            Workouts = workouts.Count(),
            Sets = sets.Count(),
            Reps = (int)repsSum,
            Weight = (int)weightSum,
            Duration = duration
        };

        return View(statisticsVM);
    }
    private async Task<List<Workout>> GetWorkouts(AppUser user)
    {
        var workouts = await _context.Workouts.Include(x=>x.WorkoutSegments).Where(x => x.AppUserId == user.Id).ToListAsync();
        return workouts;
    }
    private async Task<List<Set>> GetSets(List<Workout> workouts)
    {
        var segmentsId = new List<int>();
        var sets = new List<Set>();
        foreach(var workout in workouts)
        {
            if(workout.WorkoutSegments!=null)
            foreach(var segment in workout.WorkoutSegments)
            {

                sets.AddRange(await _context.Sets.Where(x => x.WorkoutSegmentId == segment.Id).ToListAsync());
            }
        }
        return sets;
    }
}
