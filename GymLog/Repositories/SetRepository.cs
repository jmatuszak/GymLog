using GymLog.Data;
using GymLog.Interfaces;
using GymLog.Models;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Repositories
{
    public class SetRepository : ISetRepository
    {
        private readonly AppDbContext _context;

        public SetRepository(AppDbContext context)
        {
            _context = context;
        }


        public void Delete(Set set)
        {
            _context.Sets.Remove(set);
        }

        public async Task<Set> GetByIdAsync(int id)
        {
            var set = await _context.Sets.FirstOrDefaultAsync(s => s.Id == id);
            return set;
        }

        public async Task<IEnumerable<Set>> GetListAsync()
        {
            var sets = await _context.Sets.ToListAsync();
            return sets;
        }

        public void Insert(Set set)
        {
            _context.Sets.Add(set);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Set set)
        {
            _context.Update(set);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<Set>> GetSetWithWorkoutTemplateIdListAsync(int id)
        {
            return await _context.Sets.Where(x => x.WorkoutSegmentId == id).ToListAsync();
        }

        public void DeleteRange(IEnumerable<Set> sets)
        {
            _context.RemoveRange(sets);
        }
    }
}
