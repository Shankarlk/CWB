using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.CompanySettings.Domain;
using CWB.CompanySettings.Infrastructure;

namespace CWB.CompanySettings.Repositories.Location
{
    public class NoOfDaysTimeSlotRepository : Repository<NoOfDaysTimeSlot>, INoOfDaysTimeSlotRepository
    {
        public NoOfDaysTimeSlotRepository(CompanySettingsDbContext context)
         : base(context)
        { }
    }
}
