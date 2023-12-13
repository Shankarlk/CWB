using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Domain;

namespace CWB.Masters.Repositories.ItemMaster
{
    public interface IMPBOMRepository : IRepository<Domain.MPBOM>
    {
        public bool AddObj(MPBOM mPMakeFrom);
        public bool RemObj(MPBOM mPMakeFrom);
    }
}
