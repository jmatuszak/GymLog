using GymLog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace GymLog.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Excercise> Excercises { get; set; }
        public DbSet<BodyPart> BodyParts { get; set; }
        public DbSet<BodyPartExcercise> BodyPartExcercises { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<TemplateSegment> TemplateSegments { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BodyPart>()
                .HasMany(b => b.Excercises)
                .WithMany(b => b.BodyParts)
                .UsingEntity<BodyPartExcercise>(
                    j => j
                        .HasOne(be => be.Excercise)
                        .WithMany(e => e.BodyPartExcercises)
                        .HasForeignKey(be => be.ExcerciseId),
                    j => j
                        .HasOne(be => be.BodyPart)
                        .WithMany(p => p.BodyPartExcercises)
                        .HasForeignKey(be => be.BodyPartId)
                    );


        }
    }
}
