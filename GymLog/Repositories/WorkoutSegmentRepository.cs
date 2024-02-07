using GymLog.Data;
using GymLog.Interfaces;
using GymLog.Models;
using GymLog.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Repositories
{
    public class WorkoutSegmentRepository : IWorkoutSegmentRepository
    {
        private readonly AppDbContext _context;

        public WorkoutSegmentRepository(AppDbContext context)
        {
            _context = context;
        }


        public void Delete(WorkoutSegment workoutSegment)
        {
            _context.WorkoutSegments.Remove(workoutSegment);
        }

        public async Task<WorkoutSegment> GetByIdAsync(int id)
        {
            var segment = await _context.WorkoutSegments.Include(x => x.Sets).FirstOrDefaultAsync(w => w.Id == id);

            return segment;
        }

        public async Task<IEnumerable<WorkoutSegment>> GetListAsync()
        {
            var segments = await _context.WorkoutSegments.Include(s => s.Sets).Include(a => a.Exercise).Include(a => a.Template).ToListAsync();
            return segments;
        }

        public void Insert(WorkoutSegment workoutSegment)
        {
            _context.WorkoutSegments.Add(workoutSegment);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(WorkoutSegment workoutSegment)
        {
            _context.Update(workoutSegment);
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
    }
}
