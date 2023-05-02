using GymLog.Models;
using System.ComponentModel.DataAnnotations;

namespace GymLog.ViewModels
{
    public class WorkoutVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int TemplateId { get; set; }
        public Template? Template { get; set; }
        public List<WorkoutSegmentVM>? WorkoutSegmentsVM { get; set; }
        public List<Excercise>? Excercises { get; set; }
        public string? ActionName { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}

