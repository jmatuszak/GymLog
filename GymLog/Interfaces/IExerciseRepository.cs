using GymLog.Models;


namespace GymLog.Interfaces
{
    public interface IExerciseRepository : IDisposable
    {
        Task<Exercise> GetExerciseByIdAsync(int id);
        Task<IEnumerable<Exercise>> GetExerciseListAsync();
        void InsertExercise(Exercise exercise);
        void InsertBodyPartExercise(Exercise exercise);
        void DeleteExercise(Exercise exercise);
        void UpdateExercise(Exercise exercise);
        void Save();
        void UpdateBodyPartExercise(Exercise exercise);
    }
}
