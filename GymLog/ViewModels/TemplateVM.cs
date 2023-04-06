﻿using GymLog.Models;

namespace GymLog.ViewModels
{
	public class TemplateVM
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public List<ExcerciseSetVM>? ExcerciseSetsVM { get; set; }
        public List<CreateExcerciseConcatVM>? ExcercisesConcatVM { get; set; }
    }
}
