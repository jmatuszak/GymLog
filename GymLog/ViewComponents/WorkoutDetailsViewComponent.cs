using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymLog.Models;
using GymLog.Data;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GymLog.ViewComponents;


public class WorkoutDetailsViewComponent : ViewComponent
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public WorkoutDetailsViewComponent(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync(int? id)
    {
        var workoutVM = new WorkoutVM();
        if (id != null)
        {
            var workout = await _context.Workouts.Include(t => t.WorkoutSegments).FirstOrDefaultAsync(x => x.Id == id);
            workoutVM ??= new WorkoutVM();
            workoutVM.ActionName = "Info";
            if (workout == null) return View("Error");
            workoutVM = WorkoutToWorkoutVM(workout, workoutVM);
        }
        return View(workoutVM);
    }
    private WorkoutVM WorkoutToWorkoutVM(Workout workout, WorkoutVM workoutVM)
	{
		workoutVM.Exercises = _context.Exercises.ToList();
		workoutVM.WorkoutSegmentsVM ??= new List<WorkoutSegmentVM>();
		workoutVM.Id = workout.Id;
		workoutVM.Name = workout.Name;
		workoutVM.StartDate = workout.StartDate;
		workoutVM.EndDate = workout.EndDate;
		workoutVM.AppUserId= workout.AppUserId;

		var segmentsVM = new List<WorkoutSegmentVM>();
		if (workout.WorkoutSegments != null && workout.WorkoutSegments.Count > 0)
		{
			for (int t = 0; t < workout.WorkoutSegments.Count; t++)
			{
				workout.WorkoutSegments[t].Sets = _context.Sets.Where(s => s.WorkoutSegmentId == workout.WorkoutSegments[t].Id).ToList();
				var setsVM = new List<SetVM>();
				var segmentVM = new WorkoutSegmentVM();
				segmentVM.Id = workout.WorkoutSegments[t].Id;
				segmentVM.TemplateId = workout.WorkoutSegments[t].TemplateId;
				segmentVM.ExerciseId = workout.WorkoutSegments[t].ExerciseId;
				segmentVM.Description = workout.WorkoutSegments[t].Description;
				segmentVM.WeightType = workout.WorkoutSegments[t].WeightType;
				if (workout.WorkoutSegments[t].Sets != null)
				{
					for (var s = 0; s < workout.WorkoutSegments[t].Sets.Count; s++)
					{
						var setVM = new SetVM();

						if (workout.WorkoutSegments[t].Sets[s].Id != null)
							setVM.Id = workout.WorkoutSegments[t].Sets[s].Id;
						setVM.Description = workout.WorkoutSegments[t].Sets[s].Description;
						setVM.Weight = workout.WorkoutSegments[t].Sets[s].Weight;
						setVM.Reps = workout.WorkoutSegments[t].Sets[s].Reps;
						setVM.WorkoutSegmentId = workout.WorkoutSegments[t].Sets[s].WorkoutSegmentId;
						if (workoutVM.ActionName.Equals("Edit")) setVM.isDone = true;
						setsVM.Add(setVM);
					}
				}
				segmentVM.SetsVM = setsVM;
				workoutVM.WorkoutSegmentsVM.Add(segmentVM);
			}
		}
		return workoutVM;
	}
}
