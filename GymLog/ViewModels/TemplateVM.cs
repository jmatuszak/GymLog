using GymLog.Models;
using System.ComponentModel.DataAnnotations;

namespace GymLog.ViewModels
{
	public class TemplateVM
	{
		public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50)]
        public string? Name { get; set; }
		public List<WorkoutSegmentVM>? WorkoutSegmentsVM { get; set; }
        public List<Exercise>? Exercises { get; set; }
        public string? ActionName { get; set; }
        public string? AppUserId { get; set; }
        public string? AppUserEmail { get; set; }
    }
}
