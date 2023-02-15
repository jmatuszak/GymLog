using GymLog.Models;

namespace GymLog.Interfaces
{
    public interface IBodyPartRepository
    {
        Task<IEnumerable<BodyPart>> GetAll();
        Task<BodyPart> GetById(int id);
        Task<IEnumerable<BodyPart>> GetByName(string name);
        bool Add(BodyPart bodyPart);
        bool Update(BodyPart bodyPart);
        bool Delete(BodyPart bodyPart);
        bool Save();
    }
}
