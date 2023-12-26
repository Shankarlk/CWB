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

        public async Task<IEnumerable<RawMateriaTypeVM>> GetRMTypes()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/rmtypes");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<RawMateriaTypeVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<RawMaterialSpecVM>> GetRMSpecs()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/rmspecs");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<RawMaterialSpecVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<RawMaterialStandardVM>> GetRMStandards()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/rmstandards");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<RawMaterialStandardVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<BaseRawMatrialVM>> GetBaseRMs()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/baserms");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<BaseRawMatrialVM>>.GetAsync(uri, headers);
        }




        public async Task<IEnumerable<ManufacturedPartNoDetailVM>> GetManufacturedPartNoDetailList(long ManufPartType,string companyName)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/getmanufacturedpartnodetailList/{ManufPartType}/{companyName}/{tenantId}");
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

        public async Task<IEnumerable<PartStatusVM>> GetStatuses()
        {
            var uri = new Uri(_apiUrls.Gateway + "/cwbms/statuses");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<PartStatusVM>>.GetAsync(uri, headers);
        }

        public async Task<ManufacturedPartNoDetailVM> ManufacturedPartNoDetail(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/manufacturedpartnodetail");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            manufacturedPartNoDetailVM.TenantId = tenantId;
            return await RestHelper<ManufacturedPartNoDetailVM>.PostAsync(uri, manufacturedPartNoDetailVM, headers);
        }
        public async Task<MPMakeFromVM> MPMakeFrom(MPMakeFromVM manufacturedPartNoDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/mpmakefrom");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            manufacturedPartNoDetailVM.TenantId = tenantId;
            return await RestHelper<MPMakeFromVM>.PostAsync(uri, manufacturedPartNoDetailVM, headers);
        }
        public async Task<MPBomVM> MPBOM(MPBomVM manufacturedPartNoDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/mpbom");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            manufacturedPartNoDetailVM.TenantId = tenantId;
            return await RestHelper<MPBomVM>.PostAsync(uri, manufacturedPartNoDetailVM, headers);
        }
        public async Task<IEnumerable<MPMakeFromVM>> GetMPMakeFromListByPartId(string partId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/mpmakefromlist/{partId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<MPMakeFromVM>>.GetAsync(uri, headers);
        }

        public async Task<MPMakeFromVM> GetMakeFrom(string id)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/getmakefrom/{id}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<MPMakeFromVM>.GetAsync(uri, headers);
        }

        public async Task<MPBomVM> GetBOM(string id)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/getbom/{id}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<MPBomVM>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<MPBomVM>> BOMS(string partId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/boms/{partId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<MPBomVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<BoughtOutFinishDetailVM>> BOFS()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/bofs/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<BoughtOutFinishDetailVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<RawMaterialDetailVM>> SupplierRMS(string supplierId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/supplierrms/{supplierId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<RawMaterialDetailVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<ManufacturedPartNoDetailVM>> GetAllManufacturedPartNoDetailList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/mfdlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<ManufacturedPartNoDetailVM>>.GetAsync(uri, headers);
        }

        public async Task<bool> CheckPartNo(string partNo)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/check-partno/{partNo}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }

        public async Task<ManufacturedPartNoDetailVM> GetManufPart(int partId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/getmanufpart/{partId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<ManufacturedPartNoDetailVM>.GetAsync(uri, headers);
            //
        }
        public async Task<RawMaterialDetailVM> GetRMPart(int partId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/getrmpart/{partId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<RawMaterialDetailVM>.GetAsync(uri, headers);
        }

        public async Task<BoughtOutFinishDetailVM> GetBOFPart(int partId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/getbofpart/{partId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<BoughtOutFinishDetailVM>.GetAsync(uri, headers);
        }

        


        public async Task<RawMaterialDetailVM> RawMaterialDetail(RawMaterialDetailVM rawMaterialDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/rawmaterialdetail");
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

        public async Task<PartPurchaseDetailsVM> PartPurchase(PartPurchaseDetailsVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/partpurchase");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<PartPurchaseDetailsVM>.PostAsync(uri, purchaseDetailVM, headers);
        }

        
        

        public async Task<IEnumerable<PartPurchaseDetailsVM>> PartPurchases()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/partpurchases/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<PartPurchaseDetailsVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<PartPurchaseDetailsVM>> PartPurchasesFor(int partId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/purchasesbypartId/{partId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<PartPurchaseDetailsVM>>.GetAsync(uri, headers);
        }
        

        public async Task<PartPurchaseDetailsVM> GetPartPurchase(int partPurchaseId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/getpartpurchase/{partPurchaseId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<PartPurchaseDetailsVM>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<ItemMasterPartVM>> ItemMasterParts()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/itemmasterparts/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<ItemMasterPartVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<SelectPartVM>> SelectParts()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/selectparts/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<SelectPartVM>>.GetAsync(uri, headers);
        }

        //redundant as of 08/11/2023
        //'Search Raw Materials' uses partpurchases instead
        public async Task<IEnumerable<RawMaterialDetailVM>> OwnRMS()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/ownrms/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<RawMaterialDetailVM>>.GetAsync(uri, headers);
        }


       

        public async Task<BaseRawMatrialVM> BaseRM(BaseRawMatrialVM model)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/baserm");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            model.TenantId = tenantId;
            return await RestHelper<BaseRawMatrialVM>.PostAsync(uri, model, headers);
        }

        public async Task<RawMaterialStandardVM> RMStandard(RawMaterialStandardVM model)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/rmstandard");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            model.TenantId = tenantId;
            return await RestHelper<RawMaterialStandardVM>.PostAsync(uri, model, headers);

        }

        public async Task<RawMaterialSpecVM> RMSpec(RawMaterialSpecVM model)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/rmspec");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            model.TenantId = tenantId;
            return await RestHelper<RawMaterialSpecVM>.PostAsync(uri, model, headers);

        }

        public async Task<RawMateriaTypeVM> RMType(RawMateriaTypeVM model)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/rmtype");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            model.TenantId = tenantId;
            return await RestHelper<RawMateriaTypeVM>.PostAsync(uri, model, headers);
        }

        public async Task<MPMakeFromVM> RemMakeFrom(MPMakeFromVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/remmakefrom");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<MPMakeFromVM>.PostAsync(uri, purchaseDetailVM, headers);
        }

        public async Task<MPBomVM> RemBOM(MPBomVM bomvm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/rembom");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            bomvm.TenantId = tenantId;
            return await RestHelper<MPBomVM>.PostAsync(uri, bomvm, headers);
        }

        public async Task<PartPurchaseDetailsVM> RemPartPurchase(PartPurchaseDetailsVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/rempartpurchase");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<PartPurchaseDetailsVM>.PostAsync(uri, purchaseDetailVM, headers);
        }


    }
}
