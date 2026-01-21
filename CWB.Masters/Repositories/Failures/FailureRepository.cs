using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;

namespace CWB.Masters.Repositories.Failures
{
    public class FailureRepository : Repository<Domain.Failure>, IFailureRepository
    {
        public FailureRepository(MastersDbContext context)
         : base(context)
        { }
    }
}