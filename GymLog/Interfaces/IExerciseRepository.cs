using GymLog.Models;


namespace GymLog.Interfaces
{
    public interface IExerciseRepository : IDisposable
    {
        Task<Exercise> GetByIdAsync(int id);
        Task<IEnumerable<Exercise>> GetListAsync();
        void Insert(Exercise exercise);
        void InsertBodyPartExercise(Exercise exercise);
        void Delete(Exercise exercise);
        void Update(Exercise exercise);
        void Save();
        void UpdateBodyPartExercise(Exercise exercise);
    }
}
