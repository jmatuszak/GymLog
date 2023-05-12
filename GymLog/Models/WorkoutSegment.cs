using GymLog.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymLog.Models
{
    public class WorkoutSegment
    {
        public int Id { get; set; }
        public string? Description { get; set; }
		public int ExcerciseId { get; set; }
		public Excercise? Excercise { get; set; }
        public WeightType? WeightType { get; set; }
        public int? TemplateId { get; set; }
        public int? WorkoutId { get; set; }
        public Template? Template { get; set; }
        public Workout? Workout { get; set; }
        public List<Set>? Sets { get; set; }

	}
}
