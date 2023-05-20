using GymLog.Data.Enum;
using GymLog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GymLog.ViewModels
{
	public class WorkoutSegmentVM
	{
		public int Id { get; set; }
		public string? Description { get; set; }
        [Required(ErrorMessage = "You must choose an exercise")]
        public int ExerciseId { get; set; }
		public int? TemplateId { get; set; }
		public int? WorkoutId { get; set; }
		public List<SetVM>? SetsVM { get; set; }
		public string? ActionName { get; set; }
        public WeightType? WeightType { get; set; }


        public Exercise? Exercise { get; set; }
        public SetVM? SetVM { get; set; }
        public Template? Template { get; set; }
        public List<Exercise>? Exercises { get; set; }

    }
}
