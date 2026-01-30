using CWB.CommonUtils.Common;
using CWB.CompanySettings.CompanySettingsUtils;
using CWB.CompanySettings.Services.Location;
using CWB.CompanySettings.ViewModels.Location;
using CWB.CompanySettings.ViewModelValidators.Location;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Controllers
{

    [ApiController]
    [Authorize]
    public class PlantController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IPlantService _plantService;

        public PlantController(ILoggerManager logger, IPlantService plantService)
        {
            _logger = logger;
            _plantService = plantService;
        }

        /// <summary>
        /// Get Plants by tenant Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.Plant.GetPlants)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PlantVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetPlants(long tenantId)
        {
           // var companyTypes = _plantService.GetPlants(tenantId);
            var plants =  _plantService.GetPlants(tenantId);
            return Ok(plants);
        }

        /// <summary>
        /// Add or Edit Plant
        /// </summary>
        /// <param name="plantVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Plant.PostPlant)]
        [Produces(AppContentTypes.ContentType, Type = typeof(PlantVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostPlant([FromBody] PlantVM plantVM)
        {
            //var validator = new PlantVMValidator();
            //var validationResult = await validator.ValidateAsync(plantVM);
            //if (!validationResult.IsValid)
            //    return BadRequest(validationResult.Errors);
            //check if duplicate
            var plantExist = _plantService.CheckPlantExisit(new CheckPlantVM
            {
                PlantId = plantVM.PlantId,
                Name = plantVM.Name,
                TenantId = plantVM.TenantId
            });
            if (plantExist)
            {
                ModelState.AddModelError("Name", $"Plant: {plantVM.Name} Already Exist");
                return BadRequest(ModelState);
            }
            var result = await _plantService.Plant(plantVM);
            return Ok(result);
        }

        /// <summary>
        /// Check if plant exist
        /// </summary>
        /// <param name="checkPlantVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Plant.CheckPlant)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> CheckPlant([FromBody] CheckPlantVM checkPlantVM)
        {
            var validator = new CheckPlantVMValidator();
            var validationResult = await validator.ValidateAsync(checkPlantVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            //check if duplicate
            var plantExist = _plantService.CheckPlantExisit(checkPlantVM);
            return Ok(plantExist);
        }

        [HttpGet]
        [Route(ApiRoutes.Plant.GetPlant)]
        [Produces(AppContentTypes.ContentType, Type = typeof(PlantVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetPlant(long plantId)
        {
            var docTypes = await _plantService.GetPlant(plantId);
            return Ok(docTypes);
        }

        [HttpGet]
        [Route(ApiRoutes.Plant.DelPlant)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelPlant(long plantId)
        {
            var docTypes = _plantService.DelPlant(plantId);
            return Ok(docTypes);
        }

        [HttpGet]
        [Route(ApiRoutes.Plant.DelHoliday)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelHoliday(long holidayId)
        {
            var docTypes = _plantService.DelHoliday(holidayId);
            return Ok(docTypes);
        }
        [HttpGet]
        [Route(ApiRoutes.Plant.Holidays)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<HolidayVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult Holidays(long plantId)
        {
            var docTypes = _plantService.Holidays(plantId);
            return Ok(docTypes);
        }

        [HttpPost]
        [Route(ApiRoutes.Plant.PostHoliday)]
        [Produces(AppContentTypes.ContentType, Type = typeof(HolidayVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostHoliday([FromBody] HolidayVM holidayVM)
        {
            var existingHolidays = _plantService.Holidays(holidayVM.PlantId);

            var existingHoliday = existingHolidays.FirstOrDefault(h => h.HolidayId == holidayVM.HolidayId);

            bool isNewOrChanged = false;

            if (existingHoliday == null)
            {
                isNewOrChanged = true;
            }
            else
            {
                if (existingHoliday.HolidayDate != holidayVM.HolidayDate || existingHoliday.Name != holidayVM.Name)
                {
                    isNewOrChanged = true;
                }
            }

            if (isNewOrChanged)
            {
                var plant = await _plantService.GetPlant(holidayVM.PlantId);
                plant.Change_flag = 'Y';
               var updateplant =  await _plantService.Plant(plant); 
            }

            var result = await _plantService.PostHoliday(holidayVM);
            return Ok(result);
        }

        [HttpPost]
        [Route(ApiRoutes.Plant.PostPlantWD)]
        [Produces(AppContentTypes.ContentType, Type = typeof(PlantWorkingDetailsVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostPlantWD([FromBody] PlantWorkingDetailsVM plantWd)
        {
            var result = await _plantService.PostPlantWD(plantWd);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Plant.PostCity)]
        [Produces(AppContentTypes.ContentType, Type = typeof(PlantWorkingDetailsVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostCity([FromBody] CityVM cityVM)
        {
            var result = await _plantService.PostCity(cityVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Plant.PostSections)]
        [Produces(AppContentTypes.ContentType, Type = typeof(SectionsVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostSections([FromBody] SectionsVM cityVM)
        {
            var result = await _plantService.PostSections(cityVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Plant.PostCountry)]
        [Produces(AppContentTypes.ContentType, Type = typeof(PlantWorkingDetailsVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostCountry([FromBody] CountryVM cityVM)
        {
            var result = await _plantService.PostCountry(cityVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Plant.GetCitys)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PlantVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetCitys(long tenantId)
        {
            // var companyTypes = _plantService.GetPlants(tenantId);
            var plants =  await _plantService.GetCitysAsync(tenantId);
            return Ok(plants);
        }
        [HttpGet]
        [Route(ApiRoutes.Plant.GetSections)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PlantVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetSections(long tenantId)
        {
            // var companyTypes = _plantService.GetPlants(tenantId);
            var plants =  _plantService.GetSections(tenantId);
            return Ok(plants);
        }
        [HttpGet]
        [Route(ApiRoutes.Plant.GetNoOfDaysTimeSlots)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PlantVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetNoOfDaysTimeSlots()
        {
            var plants =  await _plantService.GetNoOfDaysTimeSlots();
            return Ok(plants);
        }
        [HttpGet]
        [Route(ApiRoutes.Plant.GetTimeSlotDurations)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PlantVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetTimeSlotDurations()
        {
            var plants =  await _plantService.GetTimeSlotDurations();
            return Ok(plants);
        }
        [HttpGet]
        [Route(ApiRoutes.Plant.GetCountrys)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PlantVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetCountrys(long tenantId)
        {
            // var companyTypes = _plantService.GetPlants(tenantId);
            var plants = await _plantService.GetCountrysAsync(tenantId);
            return Ok(plants);
        }

        [HttpGet]
        [Route(ApiRoutes.Plant.CheckCity)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> CheckCity(string city)
        {
            bool exists = false;
            exists = await _plantService.CheckCity(city);
            return Ok(exists);
        }
        [HttpGet]
        [Route(ApiRoutes.Plant.CheckSections)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> CheckSections(string city)
        {
            bool exists = false;
            exists = await _plantService.CheckSections(city);
            return Ok(exists);
        }
        [HttpGet]
        [Route(ApiRoutes.Plant.CheckCountrys)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> CheckCountry(string country)
        {
            bool exists = false;
            exists = await _plantService.CheckCountry(country);
            return Ok(exists);
        }

        [HttpGet]
        [Route(ApiRoutes.Plant.GetPlantWD)]
        [Produces(AppContentTypes.ContentType, Type = typeof(PlantWorkingDetailsVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetPlantWD(long tenantId,long plantId)
        {
            var result = await _plantService.GetPlantWD(tenantId, plantId);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Plant.DelSections)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelSections(long sectionId)
        {
            var docTypes = _plantService.DelSections(sectionId);
            return Ok(docTypes);
        }
    }
}
