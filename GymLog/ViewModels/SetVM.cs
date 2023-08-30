using GymLog.Models;
using System.ComponentModel.DataAnnotations;

namespace GymLog.ViewModels
{
	public class SetVM
	{
		public int Id { get; set; }
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public float? Weight { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
		public int? Reps { get; set; }
		public string? Description { get; set; }
        public bool isDone { get; set; }


        public int WorkoutSegmentId { get; set; }
		public WorkoutSegment? WorkoutSegment { get; set; }
	}
}
