using GymLog.Data;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymLog.Controllers
{
    public abstract class BaseController : Controller
    {
        protected AppDbContext Context { get; private set; }
        //private readonly UserManager<AppUser> _userManager;

        public BaseController()
        {
            Context = new AppDbContext();
        }

        protected Exercise ConvertVmToExercise(ExerciseVM exerciseVM)
        {
            Exercise exercise = new Exercise
            {
                Id = exerciseVM.Id,
                Name = exerciseVM.Name,
                AppUserId = exerciseVM.AppUserId
            };
            return exercise;
        }

        protected List<BodyPart> GetCheckedBodyParts(ExerciseVM exerciseVM)
        {
            var bodyParts = new List<BodyPart>();
            if (exerciseVM.BodyPartsVM != null)
                foreach (var itemVM in exerciseVM.BodyPartsVM)
                {
                    if (itemVM.IsChecked)
                    {
                        var bodypart = new BodyPart() { Id = itemVM.Id, Name = itemVM.Name };
                        bodyParts.Add(bodypart);
                    }
                }
            return bodyParts;
        }

        // TEMPLATE TO  TemplateVM
        protected TemplateVM TemplateToTemplateVM(Template template, TemplateVM templateVM)
        {
            templateVM.Exercises = Context.Exercises.ToList();
            templateVM.WorkoutSegmentsVM = templateVM.WorkoutSegmentsVM ?? new List<WorkoutSegmentVM>();
            templateVM.Id = template.Id;
            templateVM.Name = template.Name;

            var segmentsVM = new List<WorkoutSegmentVM>();
            if (template.WorkoutSegments != null && template.WorkoutSegments.Count > 0)
            {
                for (int t = 0; t < template.WorkoutSegments.Count; t++)
                {
                    template.WorkoutSegments[t].Sets = Context.Sets.Where(s => s.WorkoutSegmentId == template.WorkoutSegments[t].Id).ToList();
                    var setsVM = new List<SetVM>();
                    var segmentVM = new WorkoutSegmentVM();
                    segmentVM.Id = template.WorkoutSegments[t].Id;
                    segmentVM.TemplateId = template.WorkoutSegments[t].TemplateId;
                    segmentVM.ExerciseId = template.WorkoutSegments[t].ExerciseId;
                    segmentVM.Description = template.WorkoutSegments[t].Description;
                    segmentVM.WeightType = template.WorkoutSegments[t].WeightType;
                    if (template.WorkoutSegments[t].Sets != null)
                    {
                        for (var s = 0; s < template.WorkoutSegments[t].Sets.Count; s++)
                        {
                            if (template.WorkoutSegments[t].Sets[s].Id != null)
                            {
                                setsVM.Add(new SetVM()
                                {
                                    Id = template.WorkoutSegments[t].Sets[s].Id,
                                    Description = template.WorkoutSegments[t].Sets[s].Description,
                                    Weight = template.WorkoutSegments[t].Sets[s].Weight,
                                    Reps = template.WorkoutSegments[t].Sets[s].Reps,
                                    WorkoutSegmentId = template.WorkoutSegments[t].Sets[s].WorkoutSegmentId,
                                });
                            }
                            else
                            {
                                setsVM.Add(new SetVM()
                                {
                                    Description = template.WorkoutSegments[t].Sets[s].Description,
                                    Weight = template.WorkoutSegments[t].Sets[s].Weight,
                                    Reps = template.WorkoutSegments[t].Sets[s].Reps,
                                    WorkoutSegmentId = template.WorkoutSegments[t].Sets[s].WorkoutSegmentId,
                                });
                            }

                        }
                    }
                    segmentVM.SetsVM = setsVM;
                    templateVM.WorkoutSegmentsVM.Add(segmentVM);
                }
            }
            return templateVM;
        }

        protected WorkoutVM WorkoutToWorkoutVM(Workout workout, WorkoutVM workoutVM)
        {
            workoutVM.Exercises = Context.Exercises.ToList();
            workoutVM.WorkoutSegmentsVM ??= new List<WorkoutSegmentVM>();
            workoutVM.Id = workout.Id;
            workoutVM.Name = workout.Name;
            workoutVM.StartDate = workout.StartDate;
            workoutVM.EndDate = workout.EndDate;
            workoutVM.AppUserId = workout.AppUserId;
            workoutVM.TemplateId = workout.TemplateId;

            var segmentsVM = new List<WorkoutSegmentVM>();
            if (workout.WorkoutSegments != null && workout.WorkoutSegments.Count > 0)
            {
                for (int t = 0; t < workout.WorkoutSegments.Count; t++)
                {
                    workout.WorkoutSegments[t].Sets = Context.Sets.Where(s => s.WorkoutSegmentId == workout.WorkoutSegments[t].Id).ToList();
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
                            if (workoutVM.ActionName != null)
                            {
                                if (workoutVM.ActionName.Equals("Edit")) setVM.isDone = true;
                            }
                            setsVM.Add(setVM);
                        }
                    }
                    segmentVM.SetsVM = setsVM;
                    workoutVM.WorkoutSegmentsVM.Add(segmentVM);
                }
            }
            return workoutVM;
        }
        protected ExerciseVM ExerciseToExerciseVM(Exercise exercise, ExerciseVM exerciseVM)
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
            exerciseVM = new ExerciseVM()
            {
                Id = exercise.Id,
                Name = exercise.Name,
                BodyPartsVM = bodyPartsVM,
            };
            return exerciseVM;
        }

        protected ExerciseVM PrepareExerciseVMToUpdate(Exercise exercise)
        {
            var bodyParts = Context.BodyParts.ToList();

            if (exercise == null) return null;

            var bodyPartsVM = new List<BodyPartVM>();
            if (bodyParts != null)
                foreach (var part in bodyParts)
                {
                    bodyPartsVM.Add(new BodyPartVM
                    {
                        Id = part.Id,
                        Name = part.Name,
                    });
                }
            if (exercise.BodyParts != null)
                foreach (var part in exercise.BodyParts)
                {
                    bodyPartsVM.FirstOrDefault(i => i.Id == part.Id).IsChecked = true;
                }
            var exerciseVM = new ExerciseVM()
            {
                Id = exercise.Id,
                Name = exercise.Name,
                BodyPartsVM = bodyPartsVM,
            };
            return exerciseVM;
        }

        protected BodyPartVM BodyPartToBodyPartVM(BodyPart bodyPart)
        {
            var bodyPartVM = new BodyPartVM()
            {
                Id = bodyPart.Id,
                Name = bodyPart.Name
            };
            return bodyPartVM;
        }

    }


}
