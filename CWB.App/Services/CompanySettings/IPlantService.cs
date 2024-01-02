using CWB.App.Models.CoSettings;
using CWB.App.Models.Plants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.CompanySettings
{
    public interface IPlantService
    {
        Task<IEnumerable<PlantVM>> GetPlants();
        Task<PlantVM> PostPlant(PlantVM model);

        Task<bool> DelPlant (long plantId);
        Task<PlantVM> GetPlant(long plantId);
    }
}
