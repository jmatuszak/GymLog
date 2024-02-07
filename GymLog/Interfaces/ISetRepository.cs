using GymLog.Models;

namespace GymLog.Interfaces
{
    public interface ISetRepository: IDisposable
    {
        Task<Set> GetSetByIdAsync(int id);
        Task<IEnumerable<Set>> GetSetListAsync();
        void InsertSet(Set set);
        void DeleteSet(Set set);
        void UpdateSet(Set set);
        void Save();
    }
}
