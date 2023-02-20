using GymLog.Data;
using GymLog.Interfaces;
using GymLog.Models;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Repository
{
    public class BodyPartRepository : IBodyPartRepository
    {
        private readonly AppDbContext _context;

        public BodyPartRepository(AppDbContext context)
        {
            _context = context;
        }



        public bool Add(BodyPart bodyPart)
        {
            _context.BodyParts.Add(bodyPart);
            return Save();
        }

        public bool Delete(BodyPart bodyPart)
        {
            _context.Remove(bodyPart);
            return Save();
        }

        public async Task<IEnumerable<BodyPart>> GetAll()
        {
            return await _context.BodyParts.ToListAsync();
        }

		public async Task<BodyPart> GetById(int id)
		{
			return await _context.BodyParts.FirstOrDefaultAsync(i => i.Id == id);
		}
		public async Task<BodyPart> GetByIdAsNoTracking(int id)
		{
			return await _context.BodyParts.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
		}
		public async Task<IEnumerable<BodyPart>> GetByName(string name)
        {
            return await _context.BodyParts.Where(n => n.Name.Contains(name)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(BodyPart bodyPart)
        {
            _context.Update(bodyPart);
            return Save();
        }
    }
}
