using GymLog.Data.Enum;
using GymLog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace GymLog.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<BodyPart> BodyParts { get; set; }
        public DbSet<BodyPartExercise> BodyPartExercises { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<WorkoutSegment> WorkoutSegments { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BodyPart>()
                .HasMany(b => b.Exercises)
                .WithMany(b => b.BodyParts)
                .UsingEntity<BodyPartExercise>(
                    j => j
                        .HasOne(be => be.Exercise)
                        .WithMany(e => e.BodyPartExercises)
                        .HasForeignKey(be => be.ExerciseId),
                    j => j
                        .HasOne(be => be.BodyPart)
                        .WithMany(p => p.BodyPartExercises)
                        .HasForeignKey(be => be.BodyPartId)
                    );

            //SEED DATA

            //BODY PARTS
            modelBuilder.Entity<BodyPart>().HasData(
				new BodyPart()
				{
					Id = 1,
					Name = "Chest"

				},
				new BodyPart()
				{
					Id = 2,
					Name = "Back"

				},
				new BodyPart()
				{
					Id = 3,
					Name = "Legs"

				},
				new BodyPart()
				{
					Id = 4,
					Name = "Core"

				},
				new BodyPart()
				{
					Id = 5,
					Name = "Shoulders"

				},
				new BodyPart()
				{
					Id = 6,
					Name = "Arms"

				});

            modelBuilder.Entity<Exercise>().HasData(
				//Chest
				new Exercise()
				{
					Id = 1,
					Name = "Bench Press",
				},
				new Exercise()
				{
					Id = 2,
					Name = "Incline Bench Press",
				},
				new Exercise()
				{
					Id = 3,
					Name = "T-Bar Row",
				},
				new Exercise()
				{

					Id = 4,
					Name = "Push Ups",
				},
				new Exercise()
				{
					Id = 5,
					Name = "Wide Push Ups",
				},
				new Exercise()
				{
					Id = 6,
					Name = "Diamond Push Ups",
				},
				new Exercise()
				{
					Id = 7,
					Name = "Archer Push Ups",
				},
				new Exercise()
				{
					Id = 8,
					Name = "Decline PushUps",
				},
				new Exercise()
				{
					Id = 9,
					Name = "Dips",
				},
				new Exercise()
				{
					Id = 10,
					Name = "Ring Dips",
				},
				new Exercise()
				{
					Id = 11,
					Name = "Straight Bar Dips",
				},
				new Exercise()
				{
					Id = 12,
					Name = "Dumbell Pullover",
				},
				//Back
				new Exercise()
				{
					Id = 13,
					Name = "Single Arm Row",
				},
				new Exercise()
				{
					Id = 14,
					Name = "Bent Over Row",
				},
				new Exercise()
				{
					Id = 15,
					Name = "Single Leg Deadlift",
				},
				new Exercise()
				{
					Id = 16,
					Name = "Pulldowns Behind Head",
				},
				new Exercise()
				{
					Id = 17,
					Name = "Pull Row",
				},
				new Exercise()
				{
					Id = 18,
					Name = "Pull Ups",
				},
				//Legs
				new Exercise()
				{
					Id = 19,
					Name = "Calf Raises",
				},
				new Exercise()
				{
					Id = 20,
					Name = "Lunges",
				},
				new Exercise()
				{
					Id = 21,
					Name = "One Leg Claf Raises",
				},
				new Exercise()
				{
					Id = 22,
					Name = "Pistol Squat",
				},
				new Exercise()
				{
					Id = 23,
					Name = "Squat",
				},
				new Exercise()
				{
					Id = 24,
					Name = "Box Jumps",
				},
				new Exercise()
				{
					Id = 25,
					Name = "Deadlift",
				},
				//Core
				new Exercise()
				{
					Id = 26,
					Name = "Bridge",
				},
				new Exercise()
				{
					Id = 27,
					Name = "Hanging Knee Raises",
				},
				new Exercise()
				{
					Id = 28,
					Name = "Leg Raises",
				},
				new Exercise()
				{
					Id = 29,
					Name = "L-Sit Pull Ups",
				},
				new Exercise()
				{
					Id = 30,
					Name = "Russian Twist",
				},
				new Exercise()
				{
					Id = 31,
					Name = "Sit Ups",
				},
				new Exercise()
				{
					Id = 32,
					Name = "Toes To Bar",
				},
				new Exercise()
				{
					Id = 33,
					Name = "V-Ups",
				},
				new Exercise()
				{
					Id = 34,
					Name = "Knee Elbow Push Ups",
				},
				//Shoulders
				new Exercise()
				{
					Id = 35,
					Name = "Front Raises",
				},
				new Exercise()
				{
					Id = 36,
					Name = "Lateral Raises",
				},
				new Exercise()
				{
					Id = 37,
					Name = "Upright Row",
				},
				new Exercise()
				{
					Id = 38,
					Name = "Bent Over Reverse Fly",
				},
				new Exercise()
				{
					Id = 39,
					Name = "Military Press - OHP",
				},
				//Arms
				new Exercise()
				{
					Id = 40,
					Name = "Bicep Curl",
				},
				new Exercise()
				{
					Id = 41,
					Name = "French Press",
				},
				new Exercise()
				{
					Id = 42,
					Name = "Hummer Curls",
				},
				new Exercise()
				{
					Id = 43,
					Name = "Preacher Bicep Curl",
				},
				new Exercise()
				{
					Id = 44,
					Name = "Chin Ups",
				}
				);

			modelBuilder.Entity<BodyPartExercise>().HasData(
				new BodyPartExercise()
				{
					ExerciseId = 1,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 1,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 2,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 2,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 3,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 3,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 4,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 4,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 4,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 5,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 5,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 5,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 6,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 6,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 6,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 7,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 7,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 7,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 8,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 8,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 8,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 9,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 9,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 9,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 10,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 10,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 10,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 11,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 12,
					BodyPartId = 2

				},
				new BodyPartExercise()
				{
					ExerciseId = 13,
					BodyPartId = 2

				},
				new BodyPartExercise()
				{
					ExerciseId = 14,
					BodyPartId = 2

				},
				new BodyPartExercise()
				{
					ExerciseId = 14,
					BodyPartId = 3

				},
				new BodyPartExercise()
				{
					ExerciseId = 15,
					BodyPartId = 2

				},
				new BodyPartExercise()
				{
					ExerciseId = 15,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 16,
					BodyPartId = 2

				},
				new BodyPartExercise()
				{
					ExerciseId = 16,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 16,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 17,
					BodyPartId = 2

				},
				new BodyPartExercise()
				{
					ExerciseId = 17,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 17,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 18,
					BodyPartId = 3

				},
				new BodyPartExercise()
				{
					ExerciseId = 19,
					BodyPartId = 3

				},
				new BodyPartExercise()
				{
					ExerciseId = 20,
					BodyPartId = 3

				},
				new BodyPartExercise()
				{
					ExerciseId = 21,
					BodyPartId = 3

				},
				new BodyPartExercise()
				{
					ExerciseId = 22,
					BodyPartId = 3

				},
				new BodyPartExercise()
				{
					ExerciseId = 23,
					BodyPartId = 3

				},
				new BodyPartExercise()
				{
					ExerciseId = 23,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 24,
					BodyPartId = 2

				},
				new BodyPartExercise()
				{
					ExerciseId = 24,
					BodyPartId = 3

				},
				new BodyPartExercise()
				{
					ExerciseId = 24,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 25,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 26,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 27,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 28,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 29,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 30,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 31,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 32,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 33,
					BodyPartId = 1

				},
				new BodyPartExercise()
				{
					ExerciseId = 33,
					BodyPartId = 2

				},
				new BodyPartExercise()
				{
					ExerciseId = 33,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 34,
					BodyPartId = 5

				},
				new BodyPartExercise()
				{
					ExerciseId = 35,
					BodyPartId = 5

				},
				new BodyPartExercise()
				{
					ExerciseId = 36,
					BodyPartId = 5

				},
				new BodyPartExercise()
				{
					ExerciseId = 37,
					BodyPartId = 2

				},
				new BodyPartExercise()
				{
					ExerciseId = 37,
					BodyPartId = 5

				},
				new BodyPartExercise()
				{
					ExerciseId = 38,
					BodyPartId = 4

				},
				new BodyPartExercise()
				{
					ExerciseId = 38,
					BodyPartId = 5

				},
				new BodyPartExercise()
				{
					ExerciseId = 39,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 40,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 41,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 42,
					BodyPartId = 6

				},
				new BodyPartExercise()
				{
					ExerciseId = 43,
					BodyPartId = 2

				},
				new BodyPartExercise()
				{
					ExerciseId = 43,
					BodyPartId = 6

				}
				);

			modelBuilder.Entity<Template>().HasData(
				new Template()
				{
					Id = 1,
					Name = "Sample Workout 1",

				},
				new Template()
				{
					Id = 2,
					Name = "Sample Workout 2"
				},
				new Template()
				{
					Id = 3,
					Name = "Sample Workout 3"
				}
				);

			modelBuilder.Entity<Workout>().HasData(
				new Workout()
				{
					Id = 1,
					Name = "Sample Workout 1",
					TemplateId = 1,
				},
				new Workout()
				{
					Id = 2,
					Name = "Sample Workout 2",
					TemplateId = 2,
				},
				new Workout()
				{
					Id = 3,
					Name = "Sample Workout 3",
					TemplateId = 3,
				}
				);

			modelBuilder.Entity<WorkoutSegment>().HasData(
				// template 1
				new WorkoutSegment()
				{
					Id = 1,
					WeightType = WeightType.Barbell,
					Description = "2 x 1-4",
					TemplateId = 1,
					ExerciseId = 1,

				},
				new WorkoutSegment()
				{
					Id = 2,
					WeightType = WeightType.Barbell,
					Description = "2 x 1-4",
					TemplateId = 1,
					ExerciseId = 2,

				},
				new WorkoutSegment()
				{
					Id = 3,
					WeightType = WeightType.Dumbell,
					Description = "2 x 1-4",
					TemplateId = 1,
					ExerciseId = 3,

				},
				new WorkoutSegment()
				{
					Id = 4,
					WeightType = WeightType.Bodyweight,
					Description = "2 x 1-4",
					TemplateId = 1,
					ExerciseId = 10,

				},
				new WorkoutSegment()
				{
					Id = 5,
					WeightType = WeightType.Dumbell,
					Description = "2 x 1-4",
					TemplateId = 1,
					ExerciseId = 11,

				},
				new WorkoutSegment()
				{
					Id = 6,
					WeightType = WeightType.Dumbell,
					Description = "2 x 1-4",
					TemplateId = 1,
					ExerciseId = 12,

				},
				new WorkoutSegment()
				{
					Id = 7,
					WeightType = WeightType.Bodyweight,
					Description = "2 x 1-4",
					TemplateId = 1,
					ExerciseId = 33,

				},
				// template 2
				new WorkoutSegment()
				{
					Id = 8,
					WeightType = WeightType.Bodyweight,
					Description = "2 x 10-12",
					TemplateId = 2,
					ExerciseId = 4,

				},
				new WorkoutSegment()
				{
					Id = 9,
					WeightType = WeightType.Bodyweight,
					Description = "2 x 10-12",
					TemplateId = 2,
					ExerciseId = 6,

				},
				new WorkoutSegment()
				{
					Id = 10,
					WeightType = WeightType.Bodyweight,
					Description = "2 x 10-12",
					TemplateId = 2,
					ExerciseId = 7,

				},
				new WorkoutSegment()
				{
					Id = 11,
					WeightType = WeightType.Dumbell,
					Description = "2 x 10-12",
					TemplateId = 2,
					ExerciseId = 14,

				},
				new WorkoutSegment()
				{
					Id = 12,
					WeightType = WeightType.Dumbell,
					Description = "2 x 10-12",
					TemplateId = 2,
					ExerciseId = 15,

				},
				new WorkoutSegment()
				{
					Id = 13,
					WeightType = WeightType.Barbell,
					Description = "2 x 10-12",
					TemplateId = 2,
					ExerciseId = 24,

				},
				new WorkoutSegment()
				{
					Id = 14,
					WeightType = WeightType.Bodyweight,
					Description = "2 x 10-12",
					TemplateId = 2,
					ExerciseId = 33,

				},
				new WorkoutSegment()
				{
					Id = 15,
					WeightType = WeightType.Barbell,
					Description = "2 x 10-12",
					TemplateId = 2,
					ExerciseId = 34,

				},
				new WorkoutSegment()
				{
					Id = 16,
					WeightType = WeightType.Barbell,
					Description = "2 x 10-12",
					TemplateId = 2,
					ExerciseId = 40,

				},
				// 3
				new WorkoutSegment()
				{
					Id = 17,
					WeightType = WeightType.Dumbell,
					Description = "2 x 6-8",
					TemplateId = 3,
					ExerciseId = 1,
				},
				new WorkoutSegment()
				{
					Id = 18,
					WeightType = WeightType.Bodyweight,
					Description = "2 x 6-8",
					TemplateId = 3,
					ExerciseId = 10,
				},
				new WorkoutSegment()
				{
					Id = 19,
					WeightType = WeightType.Bodyweight,
					Description = "2 x 6-8",
					TemplateId = 3,
					ExerciseId = 20,
				},
				new WorkoutSegment()
				{
					Id = 20,
					WeightType = WeightType.Bodyweight,
					Description = "2 x 6-8",
					TemplateId = 3,
					ExerciseId = 30,
				},
				new WorkoutSegment()
				{
					Id = 21,
					WeightType = WeightType.Dumbell,
					Description = "2 x 6-8",
					TemplateId = 3,
					ExerciseId = 40,
				},
				new WorkoutSegment()
				{
					Id = 22,
					WeightType = WeightType.Dumbell,
					Description = "2 x 6-8",
					TemplateId = 3,
					ExerciseId = 41,
				}
				);

			modelBuilder.Entity<Set>().HasData(
				new Set()
				{
					Id = 1,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 1
				},
				new Set()
				{
					Id = 2,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 1
				},
				new Set()
				{
					Id = 3,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 2
				},
				new Set()
				{
					Id = 4,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 2
				},
				new Set()
				{
					Id = 5,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 3
				},
				new Set()
				{
					Id = 6,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 3
				},
				new Set()
				{
					Id = 7,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 4
				},
				new Set()
				{
					Id = 8,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 4
				},
				new Set()
				{
					Id = 9,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 5
				},
				new Set()
				{
					Id = 10,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 5
				},
				new Set()
				{
					Id = 11,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 6
				},
				new Set()
				{
					Id = 12,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 6
				},
				new Set()
				{
					Id = 13,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 7
				},
				new Set()
				{
					Id = 14,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 7
				},
				new Set()
				{
					Id = 15,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 8
				},
				new Set()
				{
					Id = 16,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 8
				},
				new Set()
				{
					Id = 17,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 9
				},
				new Set()
				{
					Id = 18,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 9
				},
				new Set()
				{
					Id = 19,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 10
				},
				new Set()
				{
					Id = 20,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 10
				},
				new Set()
				{
					Id = 21,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 11
				},
				new Set()
				{
					Id = 22,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 11
				},
				new Set()
				{
					Id = 23,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 12
				},
				new Set()
				{
					Id = 24,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 12
				},
				new Set()
				{
					Id = 25,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 13
				},
				new Set()
				{
					Id = 26,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 13
				},
				new Set()
				{
					Id = 27,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 14
				},
				new Set()
				{
					Id = 28,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 14
				},
				new Set()
				{
					Id = 29,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 15
				},
				new Set()
				{
					Id = 30,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 15
				},
				new Set()
				{
					Id = 31,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 16
				},
				new Set()
				{
					Id = 32,
					Reps = 12,
					Weight = 0,
					WorkoutSegmentId = 16
				},
				new Set()
				{
					Id = 33,
					Reps = 8,
					Weight = 0,
					WorkoutSegmentId = 17
				},
				new Set()
				{
					Id = 34,
					Reps = 8,
					Weight = 0,
					WorkoutSegmentId = 17
				},
				new Set()
				{
					Id = 35,
					Reps = 8,
					Weight = 0,
					WorkoutSegmentId = 18
				},
				new Set()
				{
					Id = 36,
					Reps = 8,
					Weight = 0,
					WorkoutSegmentId = 18
				},
				new Set()
				{
					Id = 37,
					Reps = 8,
					Weight = 0,
					WorkoutSegmentId = 19
				},
				new Set()
				{
					Id = 38,
					Reps = 4,
					Weight = 0,
					WorkoutSegmentId = 19
				},
				new Set()
				{
					Id = 39,
					Reps = 8,
					Weight = 0,
					WorkoutSegmentId = 20
				},
				new Set()
				{
					Id = 40,
					Reps = 8,
					Weight = 0,
					WorkoutSegmentId = 20
				},
				new Set()
				{
					Id = 41,
					Reps = 8,
					Weight = 0,
					WorkoutSegmentId = 21
				},
				new Set()
				{
					Id = 42,
					Reps = 8,
					Weight = 0,
					WorkoutSegmentId = 21
				},
				new Set()
				{
					Id = 43,
					Reps = 8,
					Weight = 0,
					WorkoutSegmentId = 22
				},
				new Set()
				{
					Id = 44,
					Reps = 8,
					Weight = 0,
					WorkoutSegmentId = 22
				}
			);

			base.OnModelCreating(modelBuilder);
		}



		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                "Data Source=DESKTOP-8EM1OQK\\SQLEXPRESS;Initial Catalog=GymLog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
    }
}
