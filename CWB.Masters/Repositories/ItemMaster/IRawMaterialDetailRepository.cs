using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Domain;

namespace CWB.Masters.Repositories.ItemMaster
{
    public interface IRawMaterialDetailRepository : IRepository<Domain.RawMaterialDetail>
    {
        public void AddRawMaterial(RawMaterialDetail rawMaterial);
    }
}
