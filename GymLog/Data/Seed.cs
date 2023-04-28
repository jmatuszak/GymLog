using GymLog.Data.Enum;
using GymLog.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

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
                        },
                        new Excercise()
                        {
                            
                            Name = "Incline Bench Press",
                        },
                        new Excercise()
                        {
                            
                            Name = "Decline Bench Press",
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
                if (!context.TemplateSegments.Any())
                {
                    context.TemplateSegments.AddRange(new List<TemplateSegment>()
                    {
                        new TemplateSegment()
                        {
                            WeightType = WeightType.Barbell,
                            Order = 1,
                            Description = "3 x 1-4",
                            TemplateId = 1,
                            ExcerciseId= 1,

                        },
                        new TemplateSegment()
                        {
                            WeightType = WeightType.Dumbell,
                            Order = 2,
                            Description = "3 x 1-4",
                            TemplateId = 1,
							ExcerciseId= 2,

						},
                        new TemplateSegment()
                        {
                            WeightType = WeightType.Dumbell,
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
                            TemplateSegmentId= 1,
                        },
                        new Set()
                        {
                            Weight = 90,
                            Reps = 1,
                            Description = "Harder",
							TemplateSegmentId= 1,
						},
                        new Set()
                        {
                            Weight = 100,
                            Reps = 1,
                            Description = "Hard",
							TemplateSegmentId= 1,
						},
                        new Set()
                        {
                            Weight = 80,
                            Reps = 1,
                            Description = "Easy",
							TemplateSegmentId= 2,
						},
                        new Set()
                        {
                            Weight = 90,
                            Reps = 1,
                            Description = "Harder",
							TemplateSegmentId= 2,
						},
                        new Set()
                        {
                            Weight = 100,
                            Reps = 1,
                            Description = "Hard",
							TemplateSegmentId= 2,
						},
                        new Set()
                        {
                            Weight = 80,
                            Reps = 1,
                            Description = "Easy",
							TemplateSegmentId= 3,
						},
                        new Set()
                        {
                            Weight = 90,
                            Reps = 1,
                            Description = "Harder",
							TemplateSegmentId= 2,
						},
                        new Set()
                        {
                            Weight = 100,
                            Reps = 1,
                            Description = "Hard",
							TemplateSegmentId= 3,
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
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "jnmatuszak@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "jmatuszak",
                        Email = adminUserEmail,
                        EmailConfirmed = true,

                    };
                    await userManager.CreateAsync(newAdminUser, "Abdullah.2015");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAppUser, "Abdullah.2015");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}