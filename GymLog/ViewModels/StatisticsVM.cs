namespace GymLog.ViewModels
{
    public class StatisticsVM
    {
        public int Workouts { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public TimeSpan Duration { get; set; }
        public int Weight { get; set; }
    }
}
