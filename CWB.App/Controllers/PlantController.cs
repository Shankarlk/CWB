﻿using CWB.App.Models.CoSettings;
using CWB.App.Models.Plants;
using CWB.App.Services.CompanySettings;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class PlantController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IPlantService _plantService;

        public PlantController(ILoggerManager logger, IPlantService plantService)
        {
            _logger = logger;
            _plantService = plantService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetPlants()
        {
            var result = await _plantService.GetPlants();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Plant(PlantVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _plantService.PostPlant(model);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> PlantWd(PlantWorkingDetailsVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _plantService.PostPlantWD(model);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> PlantHoliday(HolidayVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _plantService.PlantHoliday(model);
            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetPlant(long plantId)
        {
            var result = await _plantService.GetPlant(plantId);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> DelPlant(long plantId)
        {
            var result = await _plantService.DelPlant(plantId);
            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetPlantWD(long plantId)
        {
            var result = await _plantService.GetPlantWD(plantId);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetHolidays(long plantId)
        {
            var result = await _plantService.GetHolidays(plantId);
            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> DelHoliday(long holidayId)
        {
            var result = await _plantService.DelHoliday(holidayId);
            return Json(result);
        }


    }
}
