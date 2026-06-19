using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Gro_Disp_DetRepository : Repository<Gro_Disp_Det>, IGro_Disp_DetRepository
    {
        public Gro_Disp_DetRepository(GroDbContext context)
    : base(context)
        { }
    }
}
