using GymLog.Models;

namespace GymLog.ViewModels
{
	public class SetVM
	{
		public int Id { get; set; }
		public float? Weight { get; set; }
		public int? Reps { get; set; }
		public string? Description { get; set; }
        public bool isDone { get; set; }


        public int WorkoutSegmentId { get; set; }
		public WorkoutSegment? WorkoutSegment { get; set; }
	}
}
