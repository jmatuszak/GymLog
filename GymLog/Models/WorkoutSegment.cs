using GymLog.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymLog.Models
{
    public class WorkoutSegment
    {
        public int Id { get; set; }
        public string? Description { get; set; }
		public int ExerciseId { get; set; }
		public Exercise? Exercise { get; set; }
        public WeightType? WeightType { get; set; }
        public int? TemplateId { get; set; }
        public int? WorkoutId { get; set; }
        public Template? Template { get; set; }
        public Workout? Workout { get; set; }
        public List<Set>? Sets { get; set; }

	}
}
