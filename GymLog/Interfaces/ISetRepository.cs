using GymLog.Models;

namespace GymLog.Interfaces
{
    public interface ISetRepository: IDisposable
    {
        Task<Set> GetByIdAsync(int id);
        Task<IEnumerable<Set>> GetListAsync();
        Task<IEnumerable<Set>> GetSetWithWorkoutTemplateIdListAsync(int id);
        void Insert(Set set);
        void Delete(Set set);
        void DeleteRange(IEnumerable<Set> sets);
        void Update(Set set);
        void Save();
    }
}
