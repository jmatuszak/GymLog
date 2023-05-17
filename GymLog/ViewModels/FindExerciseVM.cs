namespace GymLog.ViewModels
{
    public class FindExerciseVM
    {
        public List<ExerciseVM>? ExercisesVM { get; set; }
        public List<ExerciseVM>? SearchedExercisesVM { get; set; }
        public string? SearchString { get; set; }
    }
}
