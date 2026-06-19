using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Indent_Part_Sl_NoRepository : Repository<Indent_Part_Sl_No>, IIndent_Part_Sl_NoRepository
    {
        public Indent_Part_Sl_NoRepository(GroDbContext context)
  : base(context)
        { }
    }
}
