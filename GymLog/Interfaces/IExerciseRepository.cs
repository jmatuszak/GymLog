using GymLog.Models;
using GymLog.ViewModels;

namespace GymLog.Interfaces
{
    public interface IExerciseRepository : IDisposable
    {
        Task<IEnumerable<Exercise>> GetExerciseListAsync();
        Task<Exercise> GetExerciseByIdAsync(int id);
        void InsertExercise(Exercise exercise);
        void InsertBodyPartExercise(Exercise exercise);
        void DeleteExercise(Exercise exercise);
        void UpdateExercise(Exercise exercise);
        void Save();
        void UpdateBodyPartExercise(Exercise exercise);

    }
}
