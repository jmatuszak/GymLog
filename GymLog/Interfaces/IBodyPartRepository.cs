using GymLog.Models;

namespace GymLog.Interfaces
{
    public interface IBodyPartRepository : IDisposable
    {
        Task<BodyPart> GetBodyPartByIdAsync(int id);
        Task<IEnumerable<BodyPart>> GetBodyPartListAsync();
        void InsertBodyPart(BodyPart bodyPart);
        void DeleteBodyPart(BodyPart bodyPart);
        void UpdateBodyPart(BodyPart bodyPart);
        void Save();

    }
}
