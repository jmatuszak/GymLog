using System.ComponentModel.DataAnnotations;

namespace GymLog.ViewModels
{
    public class ExerciseVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50)]
        public string Name { get; set; }
        public List<BodyPartVM>? BodyPartsVM { get; set; }
		public string? ImageSrc { get; set; }

	}
}
