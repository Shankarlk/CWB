using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class RcCaDocTypeRepository : Repository<RcCaDocType>, IRcCaDocTypeRepository
    {
        public RcCaDocTypeRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
