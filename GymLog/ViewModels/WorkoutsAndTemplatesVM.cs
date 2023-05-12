using GymLog.Data.Enum;
using GymLog.Models;
using System.ComponentModel.DataAnnotations;

namespace GymLog.ViewModels
{
	public class WorkoutsAndTemplatesVM
	{
		public List<TemplateVM>? TemplatesVM { get; set; }
		public List<WorkoutVM>? WorkoutsVM { get; set; }


    }
}
