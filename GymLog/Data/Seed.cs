﻿using GymLog.Data.Enum;
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



                if (!context.Templates.Any())
                {
                    context.Templates.AddRange(new List<Template>()
                    {
                        new Template()
                        {
                            Name = "Chest distruction",

                        },

                    });
                    context.SaveChanges();
                }
                if (!context.ExcerciseSets.Any())
                {
                    context.ExcerciseSets.AddRange(new List<ExcerciseSet>()
                    {
                        new ExcerciseSet()
                        {
                            Order = 1,
                            Description = "3 x 1-4",
                            TemplateId = 1,
                            ExcerciseId= 1,

                        },
                        new ExcerciseSet()
                        {
                            Order = 2,
                            Description = "3 x 1-4",
                            TemplateId = 1,
							ExcerciseId= 2,

						},
                        new ExcerciseSet()
                        {
                            Order = 3,
                            Description = "3 x 1-4",
                            TemplateId = 1,
							ExcerciseId= 3,

						},

                    });
                    context.SaveChanges();
                }
                if (!context.Sets.Any())
                {
                    context.Sets.AddRange(new List<Set>()
                    {
                        new Set()
                        {
                            Weight = 80,
                            Reps = 1,
                            Description = "Easy",
                            ExcerciseSetId= 1,
                        },
                        new Set()
                        {
                            Weight = 90,
                            Reps = 1,
                            Description = "Harder",
							ExcerciseSetId= 1,
						},
                        new Set()
                        {
                            Weight = 100,
                            Reps = 1,
                            Description = "Hard",
							ExcerciseSetId= 1,
						},
                        new Set()
                        {
                            Weight = 80,
                            Reps = 1,
                            Description = "Easy",
							ExcerciseSetId= 2,
						},
                        new Set()
                        {
                            Weight = 90,
                            Reps = 1,
                            Description = "Harder",
							ExcerciseSetId= 2,
						},
                        new Set()
                        {
                            Weight = 100,
                            Reps = 1,
                            Description = "Hard",
							ExcerciseSetId= 2,
						},
                        new Set()
                        {
                            Weight = 80,
                            Reps = 1,
                            Description = "Easy",
							ExcerciseSetId= 3,
						},
                        new Set()
                        {
                            Weight = 90,
                            Reps = 1,
                            Description = "Harder",
							ExcerciseSetId= 2,
						},
                        new Set()
                        {
                            Weight = 100,
                            Reps = 1,
                            Description = "Hard",
							ExcerciseSetId= 3,
						},
                    });
                    context.SaveChanges();
                }
                if (!context.Workouts.Any())
                {
                    context.Workouts.AddRange(new List<Workout>()
                    {
                        new Workout()
                        {
                            Date = DateTime.Now,
                            TemplateId = 1,

                        },

                    });
                    context.SaveChanges();
                }

            }

        }
    }
}