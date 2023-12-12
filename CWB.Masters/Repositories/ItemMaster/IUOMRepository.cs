using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Domain;
using CWB.Masters.ViewModels.ItemMaster;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Repositories.ItemMaster
{
    public interface IUOMRepository : IRepository<Domain.UOM>
    {
        public IEnumerable<UOM> GetUOMs();
    }
}
