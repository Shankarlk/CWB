using AutoMapper;
using CWB.CommonUtils.Common;
using CWB.Masters.Domain;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.ItemMaster;
using CWB.Masters.ViewModels.Machines;
using CWB.Masters.ViewModels.OperationList;
using CWB.Masters.ViewModels.Routing;

namespace CWB.Masters.MastersUtils
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Division, CompaniesVM>()
                .ForMember(m => m.DivisionId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.CompanyId, m => m.MapFrom(src => src.Company.Id))
                .ForMember(m => m.CompanyType, m => m.MapFrom(src => src.Company.Type.ToString()))
                .ForMember(m => m.CompanyName, m => m.MapFrom(src => src.Company.Name))
                .ForMember(m => m.DivisionName, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.Location, m => m.MapFrom(src => src.Location))
                .ForMember(m => m.Notes, m => m.MapFrom(src => src.Notes));
            CreateMap<CompanyVM, Division>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.DivisionId))
                .ForMember(s => s.Name, s => s.MapFrom(src => src.DivisionName));
            CreateMap<CompanyVM, Domain.Company>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.CompanyId))
                .ForMember(s => s.Name, s => s.MapFrom(src => src.CompanyName))
                .ForMember(s => s.Type, s => s.MapFrom(src => src.CompanyType.ToEnum<CompanyType>()));

            CreateMap<Domain.OperationList, OperationListVM>()
                .ForMember(m => m.OperationId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.Operation, m => m.MapFrom(src => src.Operation));

            CreateMap<OperationVM, Domain.OperationList>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.OperationId));
            CreateMap<Domain.OperationList, OperationVM>()
                .ForMember(m => m.OperationId, m => m.MapFrom(src => src.Id));
            CreateMap<OperationalDocument, OperationalDocumentListVM>()
                .ForMember(m => m.OperationalDocumentId, m => m.MapFrom(src => src.Id));

            CreateMap<OperationalDocumentListVM, OperationalDocument>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.OperationalDocumentId));

            CreateMap<MachineTypeVM, MachineType>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.MachineTypeTypeId))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.MachineTypeName));
            CreateMap<MachineType, MachineTypeVM>()
                .ForMember(m => m.MachineTypeTypeId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.MachineTypeName, m => m.MapFrom(src => src.Name));

            CreateMap<MachineVM, Machine>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.MachineMachineId))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.MachineMachineName))
                .ForMember(m => m.SlNo, m => m.MapFrom(src => src.MachineMachineSlNo))
                .ForMember(m => m.Manufacturer, m => m.MapFrom(src => src.MachineMachineManufacturer))
                .ForMember(m => m.ShopId, m => m.MapFrom(src => src.MachineDepartmentId))
                .ForMember(m => m.PlantId, m => m.MapFrom(src => src.MachinePlantId))
                .ForMember(m => m.OperationListId, m => m.MapFrom(src => src.MachineOperationListId))
                .ForMember(m => m.MachineTypeId, m => m.MapFrom(src => src.MachineMachineTypeId));

            CreateMap<Machine, MachineVM>()
                .ForMember(m => m.MachineMachineId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.MachineMachineName, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.MachineMachineSlNo, m => m.MapFrom(src => src.SlNo))
                .ForMember(m => m.MachineMachineManufacturer, m => m.MapFrom(src => src.Manufacturer))
                .ForMember(m => m.MachineDepartmentId, m => m.MapFrom(src => src.ShopId))
                .ForMember(m => m.MachinePlantId, m => m.MapFrom(src => src.PlantId))
                .ForMember(m => m.MachineOperationListId, m => m.MapFrom(src => src.OperationListId))
                .ForMember(m => m.MachineMachineTypeId, m => m.MapFrom(src => src.MachineTypeId));

            CreateMap<Machine, MachineListVM>()
                .ForMember(m => m.MachineId, m => m.MapFrom(src => src.Id));

            CreateMap<MachineProcessDocument, MachineProcDocumentListVM>()
                .ForMember(m => m.MachineProcDocumentId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.MachineProcDocumentTypeId, m => m.MapFrom(src => src.DocumentTypeId))
                .ForMember(m => m.IsMachineProcDocumentMandatory, m => m.MapFrom(src => src.IsMandatory));

            CreateMap<MachineProcDocumentVM, MachineProcessDocument>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.MachineProcDocumentId))
                .ForMember(m => m.DocumentTypeId, m => m.MapFrom(src => src.MachineProcDocumentTypeId))
                .ForMember(m => m.IsMandatory, m => m.MapFrom(src => src.IsMachineProcDocumentMandatory))
                .ForMember(m => m.MachineId, m => m.MapFrom(src => src.MachineProcDocumentMachineId));

            CreateMap<ManufacturedPartNoDetailVM, Domain.ItemMaster.MasterPart>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.PartId))
                .ForMember(s => s.PartNo, s => s.MapFrom(src => src.PartNo))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason));

            CreateMap<BoughtOutFinishDetailVM, Domain.ItemMaster.MasterPart>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.PartId))
                .ForMember(s => s.PartNo, s => s.MapFrom(src => src.PartNo))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason));

            CreateMap<RawMaterialDetailVM, Domain.ItemMaster.MasterPart>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.PartId))
                .ForMember(s => s.PartNo, s => s.MapFrom(src => src.PartNo))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason));

            CreateMap<Domain.ItemMaster.MasterPart, ManufacturedPartNoDetailVM>()
                .ForMember(s => s.PartId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.PartNo, s => s.MapFrom(src => src.PartNo))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason));

            CreateMap<Domain.ItemMaster.MasterPart, BoughtOutFinishDetailVM>()
                .ForMember(s => s.PartId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.PartNo, s => s.MapFrom(src => src.PartNo))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason));


            CreateMap<Domain.ItemMaster.MasterPart, RawMaterialDetailVM>()
                .ForMember(s => s.PartId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.PartNo, s => s.MapFrom(src => src.PartNo))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason));


            CreateMap<ManufacturedPartNoDetailVM, Domain.ManufacturedPartNoDetail>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.ManufacturedPartNoDetailId))
                .ForMember(s => s.ManufacturedPartType, s => s.MapFrom(src => src.ManufacturedPartType))
                .ForMember(s => s.PartId, s => s.MapFrom(src => src.PartId))
                .ForMember(s => s.CompanyId, s => s.MapFrom(src => src.CompanyId))
                .ForMember(s => s.FinishedWeight, s => s.MapFrom(src => src.FinishedWeight))
                .ForMember(s => s.UOMId, s => s.MapFrom(src => src.UOMId));
            /**.ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
            .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
            .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
            .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
            .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason));*/

            CreateMap<Domain.ManufacturedPartNoDetail, ManufacturedPartNoDetailVM>()
                .ForMember(s => s.ManufacturedPartNoDetailId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.ManufacturedPartType, s => s.MapFrom(src => src.ManufacturedPartType))
                .ForMember(s => s.PartId, s => s.MapFrom(src => src.PartId))
                .ForMember(s => s.CompanyId, s => s.MapFrom(src => src.CompanyId))
                .ForMember(s => s.FinishedWeight, s => s.MapFrom(src => src.FinishedWeight))
                .ForMember(s => s.UOMId, s => s.MapFrom(src => src.UOMId));
                
            /** .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason));*/


            CreateMap<RawMaterialTypeVM, Domain.ItemMaster.RawMaterialType>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.RawMaterialTypeId))
                .ForMember(s => s.Name, s => s.MapFrom(src => src.Name));

            CreateMap<Domain.ItemMaster.RawMaterialType, RawMaterialTypeVM>()
                .ForMember(s => s.RawMaterialTypeId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.Name, s => s.MapFrom(src => src.Name));


            CreateMap<RawMaterialStandardVM, Domain.ItemMaster.RawMaterialStandard>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.Standard))
                .ForMember(s => s.Name, s => s.MapFrom(src => src.Name));

            CreateMap<Domain.ItemMaster.RawMaterialStandard, RawMaterialStandardVM>()
                .ForMember(s => s.Standard, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.Name, s => s.MapFrom(src => src.Name));

            CreateMap<RawMaterialSepcVM, Domain.ItemMaster.RawMaterialSpec>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.MaterialSpecId))
                .ForMember(s => s.Name, s => s.MapFrom(src => src.Name));

            CreateMap<Domain.ItemMaster.RawMaterialSpec, RawMaterialSepcVM>()
                .ForMember(s => s.MaterialSpecId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.Name, s => s.MapFrom(src => src.Name));

            CreateMap<BaseRawMaterialVM, Domain.ItemMaster.BaseRawMaterial>()
            .ForMember(s => s.Id, s => s.MapFrom(src => src.BaseRawMaterialId))
            .ForMember(s => s.Name, s => s.MapFrom(src => src.Name));

            CreateMap<Domain.ItemMaster.BaseRawMaterial, BaseRawMaterialVM>()
                .ForMember(s => s.BaseRawMaterialId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.Name, s => s.MapFrom(src => src.Name));


            CreateMap<PartPurchaseDetailsVM, Domain.ItemMaster.PartPurchaseDetails>()
            .ForMember(s => s.Id, s => s.MapFrom(src => src.PartPurchaseId))
            .ForMember(s => s.SupplierId, s => s.MapFrom(src => src.PSupplierId))
            .ForMember(s => s.Supplier, s => s.MapFrom(src => src.PSupplier))
            .ForMember(s => s.SupplierPartNo, s => s.MapFrom(src => src.PSupplierPartNo))
            .ForMember(s => s.LeadTimeInDays, s => s.MapFrom(src => src.LeadTimeInDays))
            .ForMember(s => s.MinimumOrderQuantity, s => s.MapFrom(src => src.MinimumOrderQuantity))
            .ForMember(s => s.Price, s => s.MapFrom(src => src.Price))
            .ForMember(s => s.ShareOfBusiness, s => s.MapFrom(src => src.ShareOfBusiness))
            .ForMember(s => s.AdditionalInfo, s => s.MapFrom(src => src.PAdditionalInfo))
            .ForMember(s => s.BOFId, s => s.MapFrom(src => src.BOFId))
            .ForMember(s => s.RMId, s => s.MapFrom(src => src.RMId))
            .ForMember(s => s.PartId, s => s.MapFrom(src => src.PPartId))
            .ForMember(s => s.MasterPartType, s => s.MapFrom(src => src.PMasterPartType));

            CreateMap<Domain.ItemMaster.PartPurchaseDetails, PartPurchaseDetailsVM>()
            .ForMember(s => s.PartPurchaseId, s => s.MapFrom(src => src.Id))
            .ForMember(s => s.PSupplierId, s => s.MapFrom(src => src.SupplierId))
            .ForMember(s => s.PSupplier, s => s.MapFrom(src => src.Supplier))
            .ForMember(s => s.PSupplierPartNo, s => s.MapFrom(src => src.SupplierPartNo))
            .ForMember(s => s.LeadTimeInDays, s => s.MapFrom(src => src.LeadTimeInDays))
            .ForMember(s => s.MinimumOrderQuantity, s => s.MapFrom(src => src.MinimumOrderQuantity))
            .ForMember(s => s.Price, s => s.MapFrom(src => src.Price))
            .ForMember(s => s.ShareOfBusiness, s => s.MapFrom(src => src.ShareOfBusiness))
            .ForMember(s => s.PAdditionalInfo, s => s.MapFrom(src => src.AdditionalInfo))
            .ForMember(s => s.BOFId, s => s.MapFrom(src => src.BOFId))
            .ForMember(s => s.RMId, s => s.MapFrom(src => src.RMId))
            .ForMember(s => s.PPartId, s => s.MapFrom(src => src.PartId))
            .ForMember(s => s.PMasterPartType, s => s.MapFrom(src => src.MasterPartType));


            CreateMap<RawMaterialDetailVM, Domain.RawMaterialDetail>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.RawMaterialDetailId))
                .ForMember(s => s.RawMaterialMadeType, s => s.MapFrom(src => src.RawMaterialMadeType))
                .ForMember(s => s.RawMaterialMadeSubType, s => s.MapFrom(src => src.RawMaterialMadeSubType))
                .ForMember(s => s.PartId, s => s.MapFrom(src => src.PartId))
                .ForMember(s => s.SupplierId, s => s.MapFrom(src => src.SupplierId))
            //    .ForMember(s => s.Supplier, s => s.MapFrom(src => src.Supplier))
                .ForMember(s => s.RawMaterialTypeId, s => s.MapFrom(src => src.RawMaterialTypeId))
                .ForMember(s => s.BaseRawMaterialId, s => s.MapFrom(src => src.BaseRawMaterialId))
                .ForMember(s => s.RawMaterialWeight, s => s.MapFrom(src => src.RawMaterialWeight))
                .ForMember(s => s.RawMaterialNotes, s => s.MapFrom(src => src.RawMaterialNotes))
                .ForMember(s => s.Standard, s => s.MapFrom(src => src.Standard))
                .ForMember(s => s.MaterialSpecId, s => s.MapFrom(src => src.MaterialSpecId));

            //.ForMember(s => s.PurchaseDetailId, s => s.MapFrom(src => src.PurchaseDetailId));

            CreateMap<Domain.RawMaterialDetail, RawMaterialDetailVM>()
               .ForMember(s => s.RawMaterialDetailId, s => s.MapFrom(src => src.Id))
               .ForMember(s => s.RawMaterialMadeType, s => s.MapFrom(src => src.RawMaterialMadeType))
               .ForMember(s => s.RawMaterialMadeSubType, s => s.MapFrom(src => src.RawMaterialMadeSubType))
               .ForMember(s => s.PartId, s => s.MapFrom(src => src.PartId))
               .ForMember(s => s.SupplierId, s => s.MapFrom(src => src.SupplierId))
               //    .ForMember(s => s.Supplier, s => s.MapFrom(src => src.Supplier))
               .ForMember(s => s.RawMaterialTypeId, s => s.MapFrom(src => src.RawMaterialTypeId))
               .ForMember(s => s.BaseRawMaterialId, s => s.MapFrom(src => src.BaseRawMaterialId))
               .ForMember(s => s.RawMaterialWeight, s => s.MapFrom(src => src.RawMaterialWeight))
               .ForMember(s => s.RawMaterialNotes, s => s.MapFrom(src => src.RawMaterialNotes))
               .ForMember(s => s.Standard, s => s.MapFrom(src => src.Standard))
               .ForMember(s => s.MaterialSpecId, s => s.MapFrom(src => src.MaterialSpecId));


            CreateMap<BoughtOutFinishDetailVM, Domain.BoughtOutFinishDetail>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.BoughtOutFinishDetailId))
                .ForMember(s => s.BoughtOutFinishMadeType, s => s.MapFrom(src => src.BoughtOutFinishMadeType))
                .ForMember(s => s.PartId, s => s.MapFrom(src => src.PartId))
                .ForMember(s => s.SupplierPartNo, s => s.MapFrom(src => src.SupplierPartNo))
                .ForMember(s => s.AdditionalInfo, s => s.MapFrom(src => src.AdditionalInfo));

            CreateMap<Domain.BoughtOutFinishDetail, BoughtOutFinishDetailVM>()
               .ForMember(s => s.BoughtOutFinishDetailId, s => s.MapFrom(src => src.Id))
               .ForMember(s => s.BoughtOutFinishMadeType, s => s.MapFrom(src => src.BoughtOutFinishMadeType))
               .ForMember(s => s.PartId, s => s.MapFrom(src => src.PartId))
               .ForMember(s => s.SupplierPartNo, s => s.MapFrom(src => src.SupplierPartNo))
               .ForMember(s => s.AdditionalInfo, s => s.MapFrom(src => src.AdditionalInfo));


            CreateMap<MPMakeFromVM, Domain.MPMakeFrom>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.MPMakeFromId))
                .ForMember(s => s.PartMadeFrom, s => s.MapFrom(src => src.MPPartMadeFrom))
                .ForMember(s => s.PartId, s => s.MapFrom(src => src.MPPartId))
                .ForMember(s => s.InputWeight, s => s.MapFrom(src => src.InputWeight))
                .ForMember(s => s.ScrapGenerated, s => s.MapFrom(src => src.ScrapGenerated))
                .ForMember(s => s.QuantityPerInput, s => s.MapFrom(src => src.QuantityPerInput))
                .ForMember(s => s.YieldNotes, s => s.MapFrom(src => src.YieldNotes))
                .ForMember(s => s.PreferedRawMaterial, s => s.MapFrom(src => src.PreferedRawMaterial))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.MFDescription))
                .ForMember(s => s.ManufPartId, s => s.MapFrom(src => src.ManufPartId));

            CreateMap<Domain.MPMakeFrom, MPMakeFromVM>()
             .ForMember(s => s.MPMakeFromId, s => s.MapFrom(src => src.Id))
             .ForMember(s => s.MPPartMadeFrom, s => s.MapFrom(src => src.PartMadeFrom))
             .ForMember(s => s.MPPartId, s => s.MapFrom(src => src.PartId))
             .ForMember(s => s.InputWeight, s => s.MapFrom(src => src.InputWeight))
             .ForMember(s => s.ScrapGenerated, s => s.MapFrom(src => src.ScrapGenerated))
             .ForMember(s => s.QuantityPerInput, s => s.MapFrom(src => src.QuantityPerInput))
             .ForMember(s => s.YieldNotes, s => s.MapFrom(src => src.YieldNotes))
             .ForMember(s => s.PreferedRawMaterial, s => s.MapFrom(src => src.PreferedRawMaterial))
             .ForMember(s => s.MFDescription, s => s.MapFrom(src => src.PartDescription))
             .ForMember(s => s.ManufPartId, s => s.MapFrom(src => src.ManufPartId));


            CreateMap<MPBOMVM, Domain.MPBOM>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.MPBOMId))
                .ForMember(s => s.PartId, s => s.MapFrom(src => src.BOMPartId))
                .ForMember(s => s.Quantity, s => s.MapFrom(src => src.Quantity))
                .ForMember(s => s.PartDesc, s => s.MapFrom(src => src.BOMPartDesc))
                .ForMember(s => s.ManufPartId, s => s.MapFrom(src => src.BOMManufPartId));

            CreateMap<Domain.MPBOM, MPBOMVM>()
                .ForMember(s => s.MPBOMId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.BOMPartId, s => s.MapFrom(src => src.PartId))
                .ForMember(s => s.Quantity, s => s.MapFrom(src => src.Quantity))
                .ForMember(s => s.BOMPartDesc, s => s.MapFrom(src => src.PartDesc))
                .ForMember(s => s.BOMManufPartId, s => s.MapFrom(src => src.ManufPartId));

            CreateMap<Domain.UOM, UOMVM>()
                .ForMember(s => s.Name, s => s.MapFrom(src => src.Name))
                .ForMember(s => s.UOMId, s => s.MapFrom(src => src.Id));

            CreateMap<UOMVM, Domain.UOM>()
                .ForMember(s => s.Name, s => s.MapFrom(src => src.Name))
                .ForMember(s => s.Id, s => s.MapFrom(src => src.UOMId));


            CreateMap<MasterPartVM, Domain.ItemMaster.MasterPart>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.MasterPartId))
                .ForMember(s => s.PartNo, s => s.MapFrom(src => src.PartNo))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.MasterPartType, s => s.MapFrom(src => src.MasterPartType))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate));

            CreateMap<Domain.ItemMaster.MasterPart, MasterPartVM>()
                .ForMember(s => s.MasterPartId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.PartNo, s => s.MapFrom(src => src.PartNo))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.MasterPartType, s => s.MapFrom(src => src.MasterPartType))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate));

            CreateMap<Domain.Routing, RoutingVM>()
                .ForMember(s => s.RoutingId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.OrigRoutingId, s => s.MapFrom(src => src.OrigRoutingId))
                .ForMember(s => s.RoutingName, s => s.MapFrom(src => src.RoutingName))
                .ForMember(s => s.CreationDate, s => s.MapFrom(src => src.CreationDate))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.ManufacturedPartId, s => s.MapFrom(src => src.ManufacturedPartId));

            CreateMap<RoutingVM, Domain.Routing>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.RoutingId))
                .ForMember(s => s.OrigRoutingId, s => s.MapFrom(src => src.OrigRoutingId))
                .ForMember(s => s.RoutingName, s => s.MapFrom(src => src.RoutingName))
                 .ForMember(s => s.CreationDate, s => s.MapFrom(src => src.CreationDate))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.ManufacturedPartId, s => s.MapFrom(src => src.ManufacturedPartId));

            CreateMap<Domain.RoutingStep, RoutingStepVM>()
                .ForMember(s => s.StepId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.RoutingId, s => s.MapFrom(src => src.RoutingId))
                .ForMember(s => s.StepNumber, s => s.MapFrom(src => src.StepNumber))
                .ForMember(s => s.StepDescription, s => s.MapFrom(src => src.StepDescription))
                .ForMember(s => s.StepOperation, s => s.MapFrom(src => src.RoutingStepOperation))
                .ForMember(s => s.StepLocation, s => s.MapFrom(src => src.RoutingStepLocation))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StepSequence, s => s.MapFrom(src => src.RoutingStepSequence));
            
            CreateMap<RoutingStepVM,Domain.RoutingStep>()
              .ForMember(s => s.Id, s => s.MapFrom(src => src.StepId))
              .ForMember(s => s.RoutingId, s => s.MapFrom(src => src.RoutingId))
              .ForMember(s => s.StepNumber, s => s.MapFrom(src => src.StepNumber))
              .ForMember(s => s.StepDescription, s => s.MapFrom(src => src.StepDescription))
              .ForMember(s => s.RoutingStepOperation, s => s.MapFrom(src => src.StepOperation))
              .ForMember(s => s.RoutingStepLocation, s => s.MapFrom(src => src.StepLocation))
              .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
              .ForMember(s => s.RoutingStepSequence, s => s.MapFrom(src => src.StepSequence));

            CreateMap<RoutingStepPartVM, Domain.RoutingStepPart>()
               .ForMember(s => s.Id, s => s.MapFrom(src => src.StepPartId))
               .ForMember(s => s.RoutingStepId, s => s.MapFrom(src => src.RoutingStepId))
               .ForMember(s => s.ManufacturedPartId, s => s.MapFrom(src => src.ManufacturedPartId))
               .ForMember(s => s.QuantityAssembly, s => s.MapFrom(src => src.QuantityAssembly))
               .ForMember(s => s.BOMId, s => s.MapFrom(src => src.BOMId));

            CreateMap<Domain.RoutingStepPart, RoutingStepPartVM>()
              .ForMember(s => s.StepPartId, s => s.MapFrom(src => src.Id))
              .ForMember(s => s.RoutingStepId, s => s.MapFrom(src => src.RoutingStepId))
              .ForMember(s => s.ManufacturedPartId, s => s.MapFrom(src => src.ManufacturedPartId))
              .ForMember(s => s.QuantityAssembly, s => s.MapFrom(src => src.QuantityAssembly))
              .ForMember(s => s.BOMId, s => s.MapFrom(src => src.BOMId));
        }
    }
}