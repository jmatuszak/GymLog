using Microsoft.AspNetCore.Routing.Template;

namespace GymLog.Models
{
    public class Set
    {
        public int Id { get; set; }
        public float? Weight { get; set; }
        public int? Reps { get; set; }
        public string? Description { get; set; }
        public int WorkoutSegmentId { get; set; }
        public WorkoutSegment? WorkoutSegment { get; set; }
    }
}