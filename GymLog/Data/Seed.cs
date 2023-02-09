using GymLog.Data.Enum;
using GymLog.Models;
namespace GymLog.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.BodyParts.Any())
                {
                    context.BodyParts.AddRange(new List<BodyPart>()
                    {
                        new BodyPart()
                        {
                            
                            Name = "Chest"

                         },
                        new BodyPart()
                        {
                            
                            Name = "Back"

                         },
                        new BodyPart()
                        {
                            
                            Name = "Legs"

                         },
                        new BodyPart()
                        {
                            
                            Name = "Abs"

                         },
                        new BodyPart()
                        {
                            
                            Name = "Shoulder"

                         },
                        new BodyPart()
                        {
                            
                            Name = "Biceps"

                         },
                        new BodyPart()
                        {
                            
                            Name = "Triceps"

                         },
                    });
                    context.SaveChanges();
                }
                //Excercises
                if (!context.Excercises.Any())
                {
                    context.Excercises.AddRange(new List<Excercise>()
                    {
                        new Excercise()
                        {
                            
                            Name = "Bench Press",
                            WeightType = WeightType.Barbell
                        },
                        new Excercise()
                        {
                            
                            Name = "Incline Bench Press",
                            WeightType = WeightType.Barbell
                        },
                        new Excercise()
                        {
                            
                            Name = "Decline Bench Press",
                            WeightType = WeightType.Barbell
                        },
                        new Excercise()
                        {
                            
                            Name = "Bench Press",
                            WeightType = WeightType.Dumbell
                        },
                        new Excercise()
                        {
                            
                            Name = "Incline Bench Press",
                            WeightType = WeightType.Dumbell
                        },
                        new Excercise()
                        {
                            
                            Name = "Decline Bench Press",
                            WeightType = WeightType.Dumbell
                        },
                    });
                    context.SaveChanges();
                }
                if (!context.BodyPartExcercises.Any())
                {
                    context.BodyPartExcercises.AddRange(new List<BodyPartExcercise>()
                    {
                        new BodyPartExcercise()
                        {
                            BodyPartId = 1,
                            ExcerciseId = 1,
                        },
                        new BodyPartExcercise()
                        {
                            BodyPartId = 1,
                            ExcerciseId = 2,
                        },
                        new BodyPartExcercise
                        {
                            BodyPartId = 1,
                            ExcerciseId = 3,
                        },
                    });
                    context.SaveChanges();
                }
            }

        }
    }
}