using GymLog.Models;

namespace GymLog.Interfaces
{
    public interface IWorkoutSegmentRepository : IDisposable
    {
        Task<WorkoutSegment> GetByIdAsync(int id);
        Task<IEnumerable<WorkoutSegment>> GetListAsync();
        void Insert(WorkoutSegment WorkoutSegment);
        void Delete(WorkoutSegment WorkoutSegment);
        void Update(WorkoutSegment WorkoutSegment);
        void Save();
    }
}
