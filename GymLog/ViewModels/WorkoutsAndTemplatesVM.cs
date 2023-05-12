using GymLog.Data.Enum;
using GymLog.Models;
using System.ComponentModel.DataAnnotations;

namespace GymLog.ViewModels
{
	public class WorkoutsAndTemplatesVM
	{
        public List<TemplateVM>? UserTemplatesVM { get; set; }
        public List<TemplateVM>? SampleTemplatesVM { get; set; }
        public List<WorkoutVM>? UserWorkoutsVM { get; set; }


    }
}
