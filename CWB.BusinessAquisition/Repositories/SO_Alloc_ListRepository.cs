using CWB.BusinessAquisition.Domain;
using CWB.BusinessAquisition.Infrastructure;
using CWB.CommonUtils.Common.Repositories;


namespace CWB.BusinessAquisition.Repositories
{
    public class SO_Alloc_ListRepository : Repository<SO_Alloc_List>, ISO_Alloc_ListRepository
    {
        public SO_Alloc_ListRepository(BusinessAquisitionDbContext context)
        : base(context)
        { }
    }
}
