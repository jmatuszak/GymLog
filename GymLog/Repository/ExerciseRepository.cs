using GymLog.Data;
using GymLog.Interfaces;
using GymLog.Models;

namespace GymLog.Repository
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly AppDbContext _context;

        public ExerciseRepository(AppDbContext context)
        {
            _context = context;
        }


        public bool Add(Exercise Exercise)
        {
            _context.Exercises.Add(Exercise);
            return Save();
        }

        public bool Delete(Exercise Exercise)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Exercise>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Exercise> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Exercise> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(Exercise Exercise)
        {
            throw new NotImplementedException();
        }
    }
}
