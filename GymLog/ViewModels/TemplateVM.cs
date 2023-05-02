using GymLog.Models;

namespace GymLog.ViewModels
{
	public class TemplateVM
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public List<WorkoutSegmentVM>? WorkoutSegmentsVM { get; set; }
        public List<Excercise>? Excercises { get; set; }
        public string? ActionName { get; set; }
    }
}
