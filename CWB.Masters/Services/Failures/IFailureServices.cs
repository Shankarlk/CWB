using CWB.Masters.ViewModels.FailureError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.Services.Failures
{
    public interface IFailureServices
    {
        Task<IEnumerable<FailureVM>> GetFailure(long tenantId);
        Task<FailureVM> PostFailure(FailureVM Failure);
    }
}
