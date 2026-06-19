using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class TK_DC_Inv_ContrlRepository : Repository<TK_DC_Inv_Contrl>, ITK_DC_Inv_ContrlRepository
    {
        public TK_DC_Inv_ContrlRepository(GroDbContext context)
  : base(context)
        { }
    }
}
