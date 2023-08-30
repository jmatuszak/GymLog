using System.ComponentModel.DataAnnotations;

namespace GymLog.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        public AppUser? AppUser { get; set; }
        public int? TemplateId { get; set; }
        public Template? Template { get; set; }
        public List<WorkoutSegment>? WorkoutSegments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
