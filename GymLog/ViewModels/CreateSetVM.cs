using GymLog.Models;

namespace GymLog.ViewModels
{
	public class CreateSetVM
	{
		public int Id { get; set; }
		public float? Weight { get; set; }
		public int? Reps { get; set; }
		public string? Description { get; set; }

		public int ExcerciseId { get; set; }
		public Excercise? Excercise { get; set; }

		public int OrderedSetListId { get; set; }
		public OrderedSetList? OrderedSetList { get; set; }
	}
}
