using CWB.App.AppUtils;
using CWB.App.Models.Contacts;
using CWB.App.Models.ItemMaster;
using CWB.CommonUtils.Common;
using CWB.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.Masters
{
    public class MastersServices : IMastersServices
    {
        private readonly ILoggerManager _logger;
        private readonly ApiUrls _apiUrls;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly long tenantId;

        public MastersServices(ILoggerManager logger, IOptions<ApiUrls> apiUrlsOptions, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _apiUrls = apiUrlsOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            tenantId = long.Parse(AppUtil.GetTenantId(_httpContextAccessor.HttpContext.User));
        }

        public async Task<bool> CheckIfCompanyExisit(long CompanyId, string CompanyName)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/check-company");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            var data = new { CompanyId, CompanyName, TenantId = tenantId };
            return await RestHelper<bool>.PostAsync(uri, data, headers);
        }

        public async Task<bool> CheckIfDivisionExisit(long CompanyId, long DivisionId, string DivisionName)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/check-division");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            var data = new { CompanyId, DivisionId, DivisionName, TenantId = tenantId };
            return await RestHelper<bool>.PostAsync(uri, data, headers);
        }

        public async Task<CompanyVM> Company(CompanyVM companyVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/company");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            companyVM.TenantId = tenantId;
            return await RestHelper<CompanyVM>.PostAsync(uri, companyVM, headers);
        }

        public async Task<String> HelloWorld()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/helloworld");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<String>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<UOMVM>> GetUOMs(long tenantId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/getuoms/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<UOMVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<ContactsVM>> GetCompanies()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/companies/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<ContactsVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<ManufacturedPartNoDetailVM>> GetManufacturedPartNoDetailList(long ManufPartType,string companyName)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/getmanufacturedpartnodetailList/{ManufPartType}/{companyName}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<ManufacturedPartNoDetailVM>>.GetAsync(uri, headers);
        }


        public async Task<IEnumerable<ContactsVM>> GetDivisionsByCompanyId(long Id)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/divisions/{Id}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<ContactsVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<CompanyTypeVM>> GetCompanyTypes()
        {
            var uri = new Uri(_apiUrls.Gateway + "/cwbms/company-types");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<CompanyTypeVM>>.GetAsync(uri, headers);
        }

        public async Task<ManufacturedPartNoDetailVM> ManufacturedPartNoDetail(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/manufacturedpartnodetail");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            manufacturedPartNoDetailVM.TenantId = tenantId;
            return await RestHelper<ManufacturedPartNoDetailVM>.PostAsync(uri, manufacturedPartNoDetailVM, headers);
        }
        public async Task<ManufacturedPartNoDetailVM> MPMakeFrom(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/mpmakefrom");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            manufacturedPartNoDetailVM.TenantId = tenantId;
            return await RestHelper<ManufacturedPartNoDetailVM>.PostAsync(uri, manufacturedPartNoDetailVM, headers);
        }
        public async Task<ManufacturedPartNoDetailVM> MPBOM(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/mpbom");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            manufacturedPartNoDetailVM.TenantId = tenantId;
            return await RestHelper<ManufacturedPartNoDetailVM>.PostAsync(uri, manufacturedPartNoDetailVM, headers);
        }
        public async Task<IEnumerable<MPMakeFromListVM>> GetMPMakeFromListBypartNumber(string partNumber)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/mpmakefromlist/{partNumber}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<MPMakeFromListVM>>.GetAsync(uri, headers);
        }
        public async Task<RawMaterialDetailVM> RawMaterialDetail(RawMaterialDetailVM rawMaterialDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/rawmateriadetail");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            rawMaterialDetailVM.TenantId = tenantId;
            return await RestHelper<RawMaterialDetailVM>.PostAsync(uri, rawMaterialDetailVM, headers);
        }
        public async Task<BoughtOutFinishDetailVM> BoughtOutFinishDetail(BoughtOutFinishDetailVM boughtOutFinishDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/boughtoutfinishdetail");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            boughtOutFinishDetailVM.TenantId = tenantId;
            return await RestHelper<BoughtOutFinishDetailVM>.PostAsync(uri, boughtOutFinishDetailVM, headers);
        }
    }
}
