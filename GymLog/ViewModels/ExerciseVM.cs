using GymLog.Data.Enum;
using GymLog.Models;

namespace GymLog.ViewModels
{
    public class ExerciseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BodyPartVM>? BodyPartsVM { get; set; }
		public string? ImageSrc { get; set; }

	}
}
