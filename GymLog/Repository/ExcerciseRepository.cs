using GymLog.Data;
using GymLog.Interfaces;
using GymLog.Models;

namespace GymLog.Repository
{
    public class ExcerciseRepository : IExcerciseRepository
    {
        private readonly AppDbContext _context;

        public ExcerciseRepository(AppDbContext context)
        {
            _context = context;
        }


        public bool Add(Excercise excercise)
        {
            _context.Excercises.Add(excercise);
            return Save();
        }

        public bool Delete(Excercise excercise)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Excercise>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Excercise> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Excercise> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(Excercise excercise)
        {
            throw new NotImplementedException();
        }
    }
}
