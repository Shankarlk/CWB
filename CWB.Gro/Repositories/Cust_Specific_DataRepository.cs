using CWB.CommonUtils.Common.Repositories;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;

namespace CWB.Gro.Repositories
{
    public class Cust_Specific_DataRepository : Repository<Cust_Specific_Data>, ICust_Specific_DataRepository
    {
        public Cust_Specific_DataRepository(GroDbContext context)
    : base(context)
        { }
    }
}
