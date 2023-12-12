using CWB.App.Models.Plants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.CompanySettings
{
    public interface IPlantService
    {
        Task<IEnumerable<PlantListVM>> GetPlants();
    }
}
