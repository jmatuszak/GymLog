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
        [Required(ErrorMessage = "You must choose an excercise")]
        public int ExcerciseId { get; set; }
		public int? TemplateId { get; set; }
		public List<SetVM>? SetsVM { get; set; }
		public string? ActionName { get; set; }
        public WeightType? WeightType { get; set; }


        public Excercise? Excercise { get; set; }
        public SetVM? SetVM { get; set; }
        public Template? Template { get; set; }
        public List<Excercise>? Excercises { get; set; }

    }
}
