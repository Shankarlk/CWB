using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Gro_Indent_DispHeadRepository : Repository<Gro_Indent_DispHead>, IGro_Indent_DispHeadRepository
    {
        public Gro_Indent_DispHeadRepository(GroDbContext context)
  : base(context)
        { }
    }
}
