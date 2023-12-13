using CWB.App.Models.Contacts;
using CWB.App.Models.ItemMaster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.Masters
{
    public interface IMastersServices
    {
        Task<IEnumerable<CompanyTypeVM>> GetCompanyTypes();
        Task<IEnumerable<PartStatusVM>> GetStatuses();
        Task<IEnumerable<ContactsVM>> GetCompanies();

        Task<IEnumerable<RawMateriaTypeVM>> GetRMTypes();

        Task<IEnumerable<RawMaterialSpecVM>> GetRMSpecs();

        Task<IEnumerable<RawMaterialStandardVM>> GetRMStandards();

        Task<IEnumerable<BaseRawMatrialVM>> GetBaseRMs();


        Task<IEnumerable<UOMVM>> GetUOMs(long tenantId);


        Task<IEnumerable<ManufacturedPartNoDetailVM>> GetManufacturedPartNoDetailList(long ManufPartType, string companyName);
        Task<IEnumerable<ManufacturedPartNoDetailVM>> GetAllManufacturedPartNoDetailList();
        //Task<String> HelloWorld();
        

        Task<IEnumerable<ContactsVM>> GetDivisionsByCompanyId(long Id);
        Task<CompanyVM> Company(CompanyVM companyVM);
        Task<bool> CheckIfCompanyExisit(long CompanyId, string CompanyName);
        Task<bool> CheckIfDivisionExisit(long CompanyId, long DivisionId, string DivisionName);
        Task<ManufacturedPartNoDetailVM> ManufacturedPartNoDetail(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM);
        Task<MPMakeFromVM> MPMakeFrom(MPMakeFromVM manufacturedPartNoDetailVM);
        Task<MPBomVM> MPBOM(MPBomVM manufacturedPartNoDetailVM);
        Task<IEnumerable<MPMakeFromVM>> GetMPMakeFromListByPartId(string partId);

        Task<IEnumerable<MPBomVM>> BOMS(string partId);
        Task<IEnumerable<BoughtOutFinishDetailVM>> BOFS();

        Task<RawMaterialDetailVM> RawMaterialDetail(RawMaterialDetailVM rawMaterialDetailVM);

        Task<PartPurchaseDetailsVM> PartPurchase(PartPurchaseDetailsVM partPurchaseDetailVM);
        Task<PartPurchaseDetailsVM> RemPartPurchase(PartPurchaseDetailsVM partPurchaseDetailVM);
        Task<MPMakeFromVM> RemMakeFrom(MPMakeFromVM mPMakeFromListVM);
        Task<MPBomVM> RemBOM(MPBomVM mPBomVM);

        Task<IEnumerable<PartPurchaseDetailsVM>> PartPurchases();

        Task<IEnumerable<PartPurchaseDetailsVM>> PartPurchasesFor(int partId);

        Task<PartPurchaseDetailsVM> GetPartPurchase(int partPurchaseId);
        Task<MPMakeFromVM> GetMakeFrom(string id);
        Task<MPBomVM> GetBOM(string id);

        Task<IEnumerable<ItemMasterPartVM>> ItemMasterParts();

        Task<IEnumerable<SelectPartVM>> SelectParts();

        Task<BoughtOutFinishDetailVM> BoughtOutFinishDetail(BoughtOutFinishDetailVM boughtOutFinishDetailVM);
        Task<BaseRawMatrialVM> BaseRM(BaseRawMatrialVM model);
        Task<RawMaterialStandardVM> RMStandard(RawMaterialStandardVM model);
        Task<RawMaterialSpecVM> RMSpec(RawMaterialSpecVM model);
        Task<RawMateriaTypeVM> RMType(RawMateriaTypeVM model);

        Task<IEnumerable<RawMaterialDetailVM>> OwnRMS();
        Task<IEnumerable<RawMaterialDetailVM>> SupplierRMS(string supplier);

        Task<bool> CheckPartNo(string partNo);


        Task<ManufacturedPartNoDetailVM> GetManufPart(int partId);
        Task<RawMaterialDetailVM> GetRMPart(int partId);
        Task<BoughtOutFinishDetailVM> GetBOFPart(int partId);

        //Task<BaseRawMatrialVM> BaseRM(BaseRawMatrialVM rawMatrialVM);
        //Task<RawMaterialSpecVM> 
    }
}
