namespace GymLog.ViewModels
{
    public class FindExcerciseVM
    {
        public List<ExcerciseVM>? ExcercisesVM { get; set; }
        public List<ExcerciseVM>? SearchedExcercisesVM { get; set; }
        public string? SearchString { get; set; }
    }
}
