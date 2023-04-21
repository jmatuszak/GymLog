using GymLog.Models;

namespace GymLog.ViewModels
{
	public class SetVM
	{
		public int Id { get; set; }
		public float? Weight { get; set; }
		public int? Reps { get; set; }
		public string? Description { get; set; }
        public bool IsDone { get; set; }	
        public int TemplateSegmentId { get; set; }
		public TemplateSegmentVM? TemplateSegmentVM { get; set; }
	}
}
