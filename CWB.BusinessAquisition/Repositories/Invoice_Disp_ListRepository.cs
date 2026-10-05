using CWB.BusinessAquisition.Domain;
using CWB.BusinessAquisition.Infrastructure;
using CWB.CommonUtils.Common.Repositories;


namespace CWB.BusinessAquisition.Repositories
{
    public class Invoice_Disp_ListRepository : Repository<Invoice_Disp_List>, IInvoice_Disp_ListRepository
    {
        public Invoice_Disp_ListRepository(BusinessAquisitionDbContext context)
    : base(context)
        { }
    }
}
