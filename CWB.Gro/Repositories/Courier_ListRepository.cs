using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Courier_ListRepository : Repository<Courier_List>, ICourier_ListRepository
    {
        public Courier_ListRepository(GroDbContext context)
   : base(context)
        { }
    }
}
