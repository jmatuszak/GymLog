using System.ComponentModel.DataAnnotations;

namespace GymLog.Models
{
    public class Workout
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public int TemplateId { get; set; }
        public Template? Template { get; set; }
    }
}
