using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Gro_Stock_ListRepository : Repository<Gro_Stock_List>, IGro_Stock_ListRepository
    {
        public Gro_Stock_ListRepository(GroDbContext context)
  : base(context)
        { }
    }
}
