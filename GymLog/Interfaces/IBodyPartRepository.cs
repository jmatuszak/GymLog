using GymLog.Models;

namespace GymLog.Interfaces
{
    public interface IBodyPartRepository : IDisposable
    {
        Task<BodyPart> GetByIdAsync(int id);
        Task<IEnumerable<BodyPart>> GetListAsync();
        void Insert(BodyPart bodyPart);
        void Delete(BodyPart bodyPart);
        void Update(BodyPart bodyPart);
        void Save();

    }
}
