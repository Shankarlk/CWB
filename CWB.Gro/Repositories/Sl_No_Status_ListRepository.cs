using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Sl_No_Status_ListRepository : Repository<Sl_No_Status_List>, ISl_No_Status_ListRepository
    {
        public Sl_No_Status_ListRepository(GroDbContext context) : base(context)
        {

        }
    }
}
