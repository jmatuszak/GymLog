using GymLog.Data;
using GymLog.Interfaces;
using GymLog.Models;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Repositories
{
    public class BodyPartRepository : IBodyPartRepository
    {
        private readonly AppDbContext _context;

        public BodyPartRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Delete(BodyPart bodyPart)
        {
            _context.BodyParts.Remove(bodyPart);
        }

        public async Task<BodyPart> GetByIdAsync(int id)
        {
            var bodyPart = await _context.BodyParts.FirstOrDefaultAsync(x => x.Id == id);
            return bodyPart;
        }

        public async Task<IEnumerable<BodyPart>> GetListAsync()
        {
            return await _context.BodyParts.ToListAsync();
        }

        public void Insert(BodyPart bodyPart)
        {
            _context.BodyParts.Add(bodyPart);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(BodyPart bodyPart)
        {
            _context.BodyParts.Update(bodyPart);
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
