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
        public IActionResult GetPlants(long tenantId)
        {
            var companyTypes = _plantService.GetPlants(tenantId);
            return Ok(companyTypes);
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
            var validator = new PlantVMValidator();
            var validationResult = await validator.ValidateAsync(plantVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
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
        public IActionResult GetPlant(long plantId)
        {
            var docTypes = _plantService.GetPlant(plantId);
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
    }
}
