using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;


namespace CWB.Gro.Repositories
{
    public class Gro_Stock_DetRepository : Repository<Gro_Stock_Det>, IGro_Stock_DetRepository
    {
        public Gro_Stock_DetRepository(GroDbContext context)
  : base(context)
        { }
    }
}
