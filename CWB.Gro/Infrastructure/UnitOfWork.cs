using System;
using System.Threading.Tasks;

namespace CWB.Gro.Infrastructure
{
    public class UnitOfWork:IUnitOfWork,IDisposable
    {
        private readonly GroDbContext _context;


        public UnitOfWork(GroDbContext context)
        {
            this._context = context;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
