using CWB.CompanySettings.ViewModels.Location;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.Location
{
    public interface IPlantService
    {
        IEnumerable<PlantVM> GetPlants(long TenantId);
        Task<PlantVM> Plant(PlantVM plantVM);
        bool CheckPlantExisit(CheckPlantVM checkPlantVM);

        Task<PlantVM> GetPlant(long plantId);
        Task<bool> DelPlant(long plantId);


    }
}
