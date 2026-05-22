using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Input_Resrv_ListRepository: Repository<Input_Resrv_List>, IInput_Resrv_ListRepository
    {
        public Input_Resrv_ListRepository(WODbContext context)
      : base(context)
        {

        }
    }
}
