using CWB.BusinessAquisition.Domain;
using CWB.BusinessAquisition.Infrastructure;
using CWB.CommonUtils.Common.Repositories;

namespace CWB.BusinessAquisition.Repositories
{
    public class Cust_Disp_ListRepository : Repository<Cust_Disp_List>, ICust_Disp_ListRepository
    {
        public Cust_Disp_ListRepository(BusinessAquisitionDbContext context)
       : base(context)
        { }
    }
}
