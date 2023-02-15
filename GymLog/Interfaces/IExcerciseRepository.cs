using GymLog.Models;

namespace GymLog.Interfaces
{
    public interface IExcerciseRepository
    {
        Task<IEnumerable<Excercise>> GetAll();
        Task<Excercise> GetById(int id);
        Task<Excercise> GetByName(string name);
        bool Add(Excercise excercise); 
        bool Update(Excercise excercise);
        bool Delete(Excercise excercise);
        bool Save();

    }
}
