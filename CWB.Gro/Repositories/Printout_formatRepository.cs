using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Printout_formatRepository : Repository<Printout_format>, IPrintout_formatRepository
    {
        public Printout_formatRepository(GroDbContext context)
  : base(context)
        { }
    }
}
