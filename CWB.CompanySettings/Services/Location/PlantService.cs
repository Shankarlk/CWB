using AutoMapper;
using CWB.CompanySettings.Infrastructure;
using CWB.CompanySettings.Repositories.Location;
using CWB.CompanySettings.ViewModels.Location;
using CWB.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.Location
{
    public class PlantService : IPlantService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlantRepository _plantRepository;

        public PlantService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork, IPlantRepository plantRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _plantRepository = plantRepository;

        }

        public bool CheckPlantExisit(CheckPlantVM checkPlantVM)
        {
            var plants = _plantRepository.GetRangeAsync(p => p.Name == checkPlantVM.Name &&
            p.TenantId == checkPlantVM.TenantId);
            if (!plants.Any())
            {
                return false;
            }
            return (plants.First().Id != checkPlantVM.PlantId);
        }

        public IEnumerable<PlantListVM> GetPlants(long TenantId)
        {
            var plants = _plantRepository.GetRangeAsync(p => p.TenantId == TenantId);
            return _mapper.Map<IEnumerable<PlantListVM>>(plants);
        }

        public async Task<PlantVM> Plant(PlantVM plantVM)
        {
            var plant = _mapper.Map<Domain.Plant>(plantVM);
            if (plant.Id == 0)
            {
                await _plantRepository.AddAsync(plant);
            }
            else
            {
                plant = await _plantRepository.UpdateAsync(plant.Id, plant);
            }
            await _unitOfWork.CommitAsync();
            plantVM.PlantId = plant.Id;
            return plantVM;
        }
    }
}
