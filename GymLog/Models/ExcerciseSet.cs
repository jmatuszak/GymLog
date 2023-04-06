using GymLog.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymLog.Models
{
    public class ExcerciseSet
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string? Description { get; set; }
		public int ExcerciseId { get; set; }
		public Excercise? Excercise { get; set; }
		public int TemplateId { get; set; }
        public Template? Template { get; set; }
        public List<Set>? Sets { get; set; }

	}
}
