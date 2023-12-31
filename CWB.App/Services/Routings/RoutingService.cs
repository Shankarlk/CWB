using CWB.Logging;
using CWB.App.AppUtils;
using CWB.App.Models.Contacts;
using CWB.CommonUtils.Common;
using CWB.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CWB.App.Models.Routing;


namespace CWB.App.Services.Routings
{
    public class RoutingService:IRoutingService
    {
        private readonly ILoggerManager _logger;
        private readonly ApiUrls _apiUrls;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly long tenantId;

        public RoutingService(ILoggerManager logger, IOptions<ApiUrls> apiUrlsOptions, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _apiUrls = apiUrlsOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            tenantId = long.Parse(AppUtil.GetTenantId(_httpContextAccessor.HttpContext.User));
        }

        public async Task<List<RoutingListItemVM>> GetRoutingListItems()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/routinglistitems");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<RoutingListItemVM>>.GetAsync(uri, headers);
        }

        

        public async Task<RoutingVM> Routing(RoutingVM routingVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/newrouting");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            routingVM.TenantId = (int)tenantId;
            return await RestHelper<RoutingVM>.PostAsync(uri, routingVM, headers);
        }

        public async Task<RoutingStepVM> RoutingStep(RoutingStepVM routingStepVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/routingstep");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            routingStepVM.TenantId = (int)tenantId;
            return await RestHelper<RoutingStepVM>.PostAsync(uri, routingStepVM, headers);
        }


        public async Task<RoutingStepPartVM> RoutingStepPart(RoutingStepPartVM routingStepPartVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/routingsteppart");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            routingStepPartVM.TenantId = (int)tenantId;
            return await RestHelper<RoutingStepPartVM>.PostAsync(uri, routingStepPartVM, headers);
        }

        public async Task<RoutingStepSupplierVM> RoutingStepSupplier(RoutingStepSupplierVM routingStepSupplierVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/routingstepsupplier");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            routingStepSupplierVM.TenantId = (int)tenantId;
            return await RestHelper<RoutingStepSupplierVM>.PostAsync(uri, routingStepSupplierVM, headers);
        }

        public async Task<RoutingStepMachineVM> RoutingStepMachine(RoutingStepMachineVM routingStepMachineVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/routingstepmachine");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            routingStepMachineVM.TenantId = (int)tenantId;
            return await RestHelper<RoutingStepMachineVM>.PostAsync(uri, routingStepMachineVM, headers);
        }

        public async Task<IEnumerable<RoutingVM>> Routings(int manufPartId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/routings/{manufPartId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<RoutingVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<RoutingStepVM>> RoutingSteps(int routingId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/routingsteps/{routingId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<RoutingStepVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<RoutingStepPartVM>> StepParts(int stepId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/stepparts/{stepId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<RoutingStepPartVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<RoutingStepPartVM>> StepPartsByManufId(int manufId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/steppartsbymanufid/{manufId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<RoutingStepPartVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<RoutingStepSupplierVM>> StepSuppliers(int stepId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/stepsuppliers/{stepId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<RoutingStepSupplierVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<RoutingStepMachineVM>> StepMachines(int stepId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/stepmachines/{stepId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<RoutingStepMachineVM>>.GetAsync(uri, headers);
        }




    }
}
