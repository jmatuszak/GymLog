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
        public List<Exercise>? Exercises { get; set; }
        public string? ActionName { get; set; }
        
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}

