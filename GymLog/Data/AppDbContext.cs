using GymLog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
