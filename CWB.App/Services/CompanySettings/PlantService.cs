using CWB.App.AppUtils;
using CWB.App.Models.CoSettings;
using CWB.App.Models.Plants;
using CWB.CommonUtils.Common;
using CWB.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.CompanySettings
{
    public class PlantService : IPlantService
    {
        private readonly ILoggerManager _logger;
        private readonly ApiUrls _apiUrls;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly long tenantId;

        public PlantService(ILoggerManager logger, IOptions<ApiUrls> apiUrlsOptions, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _apiUrls = apiUrlsOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            tenantId = long.Parse(AppUtil.GetTenantId(_httpContextAccessor.HttpContext.User));
        }
        public async Task<IEnumerable<PlantVM>> GetPlants()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/plants/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<PlantVM>>.GetAsync(uri, headers);
        }

        public async Task<PlantVM> PostPlant(PlantVM plantVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/plant");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            plantVM.TenantId = tenantId;
            return await RestHelper<PlantVM>.PostAsync(uri, plantVM, headers);
        }
        public async Task<bool> DelPlant(long plantId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/delplant/{plantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }

        public async Task<PlantVM> GetPlant(long plantId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/getplant/{plantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<PlantVM>.GetAsync(uri, headers);
        }
    }
}
