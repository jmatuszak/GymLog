using GymLog.Models;

namespace GymLog.ViewModels
{
	public class TemplateVM
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public List<TemplateSegmentVM>? TemplateSegmentsVM { get; set; }
        public List<CreateExcerciseConcatVM>? ExcercisesConcatVM { get; set; }

        public string? ActionName { get; set; }
    }
}
