using GymLog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymLog.ViewModels
{
	public class TemplateSegmentVM
	{
		public int Id { get; set; }
		public int Order { get; set; }
		public string? Description { get; set; }
		public int ExcerciseId { get; set; }
		public Excercise? Excercise { get; set; }
		public List<CreateExcerciseConcatVM>? ExcercisesConcatVM { get; set; }
		public int? TemplateId { get; set; }
		public Template? Template { get; set; }
		public List<SetVM>? SetsVM { get; set; }
		public SetVM? SetVM { get; set; }
	}
}
