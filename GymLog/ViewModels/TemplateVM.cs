using GymLog.Models;

namespace GymLog.ViewModels
{
	public class TemplateVM
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public List<WorkoutSegmentVM>? WorkoutSegmentsVM { get; set; }
        public List<Exercise>? Exercises { get; set; }
        public string? ActionName { get; set; }
        public string? AppUserId { get; set; }

        public string? SearchString { get; set; }

    }
}
