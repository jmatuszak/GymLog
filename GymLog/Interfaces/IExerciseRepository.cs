using GymLog.Models;

namespace GymLog.Interfaces
{
    public interface IExerciseRepository
    {
        Task<IEnumerable<Exercise>> GetAll();
        Task<Exercise> GetById(int id);
        Task<Exercise> GetByName(string name);
        bool Add(Exercise exercise); 
        bool Update(Exercise exercise);
        bool Delete(Exercise exercise);
        bool Save();

    }
}
