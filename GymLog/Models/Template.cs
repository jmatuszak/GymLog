namespace GymLog.Models
{
    public class Template
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public string? Name { get; set; }
        public List<WorkoutSegment>? WorkoutSegments { get; set; }
    }
}