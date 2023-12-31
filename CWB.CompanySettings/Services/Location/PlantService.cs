using AutoMapper;
using CWB.CompanySettings.Infrastructure;
using CWB.CompanySettings.Repositories.Location;
using CWB.CompanySettings.ViewModels.Location;
using CWB.Logging;
using System;
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

        public IEnumerable<PlantVM> GetPlants(long TenantId)
        {
            var plants = _plantRepository.GetRangeAsync(p => p.TenantId == TenantId);
            return _mapper.Map<IEnumerable<PlantVM>>(plants);
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

        public async Task<PlantVM> GetPlant(long plantId)
        {
            var plant = await _plantRepository.SingleOrDefaultAsync(d => d.Id == plantId);
            if (plant == null)
            {
                plant = new Domain.Plant { Id = -1 };
            }
            return _mapper.Map<PlantVM>(plant);
        }
        public async Task<bool> DelPlant(long plantId)
        {
            try
            {
                var plant = await _plantRepository.SingleOrDefaultAsync(d => d.Id == plantId);
                if (plant != null)
                {
                    if (plant.Id > 0)
                    {
                        _plantRepository.Remove(plant);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
