using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Domain;

namespace CWB.Masters.Repositories.ItemMaster
{
    public interface IMPMakeFromRepository : IRepository<Domain.MPMakeFrom>
    {
        public bool AddObj(MPMakeFrom mPMakeFrom);
        public bool RemObj(MPMakeFrom mPMakeFrom);
    }
}
