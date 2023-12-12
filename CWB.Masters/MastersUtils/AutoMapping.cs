using AutoMapper;
using CWB.CommonUtils.Common;
using CWB.Masters.Domain;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.ItemMaster;
using CWB.Masters.ViewModels.Machines;
using CWB.Masters.ViewModels.OperationList;

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

            CreateMap<ManufacturedPartNoDetailVM, Domain.ManufacturedPartNoDetail>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.ManufacturedPartNoDetailId))
                .ForMember(s => s.ManufacturedPartType, s => s.MapFrom(src => src.ManufacturedPartType))
                .ForMember(s => s.CompanyName, s => s.MapFrom(src => src.CompanyName))
                .ForMember(s => s.PartNumber, s => s.MapFrom(src => src.PartNumber))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.FinishedWeight, s => s.MapFrom(src => src.FinishedWeight))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.UOMId, s => s.MapFrom(src => src.UOMId))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason))
                .ForMember(s => s.MakeFrom, s => s.MapFrom(src => src.MakeFrom))
                .ForMember(s => s.BOM, s => s.MapFrom(src => src.BOM));


            CreateMap<ManufacturedPartNoDetailListVM,Domain.ManufacturedPartNoDetail>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.ManufacturedPartNoDetailId))
                .ForMember(s => s.ManufacturedPartType, s => s.MapFrom(src => src.ManufacturedPartType))
                .ForMember(s => s.CompanyName, s => s.MapFrom(src => src.CompanyName))
                .ForMember(s => s.PartNumber, s => s.MapFrom(src => src.PartNumber))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.FinishedWeight, s => s.MapFrom(src => src.FinishedWeight))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.UOMId, s => s.MapFrom(src => src.UOMId))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason))
                .ForMember(s => s.MakeFrom, s => s.MapFrom(src => src.MakeFrom))
                .ForMember(s => s.BOM, s => s.MapFrom(src => src.BOM));

            CreateMap<Domain.ManufacturedPartNoDetail,ManufacturedPartNoDetailListVM>()
                .ForMember(s => s.ManufacturedPartNoDetailId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.ManufacturedPartType, s => s.MapFrom(src => src.ManufacturedPartType))
                .ForMember(s => s.CompanyName, s => s.MapFrom(src => src.CompanyName))
                .ForMember(s => s.PartNumber, s => s.MapFrom(src => src.PartNumber))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.FinishedWeight, s => s.MapFrom(src => src.FinishedWeight))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.UOMId, s => s.MapFrom(src => src.UOMId))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason))
                .ForMember(s => s.MakeFrom, s => s.MapFrom(src => src.MakeFrom))
                .ForMember(s => s.BOM, s => s.MapFrom(src => src.BOM));

            CreateMap<RawMaterialDetailVM, Domain.RawMaterialDetail>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.RawMaterialDetailId))
                .ForMember(s => s.RawMaterialMadeType, s => s.MapFrom(src => src.RawMaterialMadeType))
                .ForMember(s => s.RawMaterialTypeId, s => s.MapFrom(src => src.RawMaterialTypeId))
                .ForMember(s => s.InHousePartNo, s => s.MapFrom(src => src.InHousePartNo))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.BaseRawMaterialId, s => s.MapFrom(src => src.BaseRawMaterialId))
                .ForMember(s => s.RawMaterialWeight, s => s.MapFrom(src => src.RawMaterialWeight))
                .ForMember(s => s.RawMaterialNotes, s => s.MapFrom(src => src.RawMaterialNotes))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.Standard, s => s.MapFrom(src => src.Standard))
                .ForMember(s => s.MaterialSpec, s => s.MapFrom(src => src.MaterialSpec))
                .ForMember(s => s.PurchaseDetailId, s => s.MapFrom(src => src.PurchaseDetailId));
          
            CreateMap<Domain.RawMaterialDetail, RawMaterialDetailListVM>()
                .ForMember(s => s.RawMaterialDetailId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.RawMaterialMadeType, s => s.MapFrom(src => src.RawMaterialMadeType))
                .ForMember(s => s.RawMaterialTypeId, s => s.MapFrom(src => src.RawMaterialTypeId))
                .ForMember(s => s.InHousePartNo, s => s.MapFrom(src => src.InHousePartNo))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.BaseRawMaterialId, s => s.MapFrom(src => src.BaseRawMaterialId))
                .ForMember(s => s.RawMaterialWeight, s => s.MapFrom(src => src.RawMaterialWeight))
                .ForMember(s => s.RawMaterialNotes, s => s.MapFrom(src => src.RawMaterialNotes))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.Standard, s => s.MapFrom(src => src.Standard))
                .ForMember(s => s.MaterialSpec, s => s.MapFrom(src => src.MaterialSpec))
                .ForMember(s => s.PurchaseDetailId, s => s.MapFrom(src => src.PurchaseDetailId));

            CreateMap<BoughtOutFinishDetailVM, Domain.BoughtOutFinishDetail>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.BoughtOutFinishDetailId))
                .ForMember(s => s.BoughtOutFinishMadeType, s => s.MapFrom(src => src.BoughtOutFinishMadeType))
                .ForMember(s => s.PartNo, s => s.MapFrom(src => src.PartNo))
                .ForMember(s => s.SupplierPartNo, s => s.MapFrom(src => src.SupplierPartNo))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.AdditionalInformation, s => s.MapFrom(src => src.AdditionalInformation))
                .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
                .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason))
                .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
                .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
                .ForMember(s => s.PurchaseDetail, s => s.MapFrom(src => src.PurchaseDetail))
                .ForMember(s => s.Supplier, s => s.MapFrom(src => src.Supplier))
                .ForMember(s => s.PurSupplierPartNo, s => s.MapFrom(src => src.PurSupplierPartNo))
                .ForMember(s => s.LeadTimeInDays, s => s.MapFrom(src => src.LeadTimeInDays))
                .ForMember(s => s.MinOrderQuantity, s => s.MapFrom(src => src.MinOrderQuantity))
                .ForMember(s => s.Price, s => s.MapFrom(src => src.Price))
                .ForMember(s => s.ShareOfBusiness, s => s.MapFrom(src => src.ShareOfBusiness))
                .ForMember(s => s.PurAdditionalInformation, s => s.MapFrom(src => src.PurAdditionalInformation));

            CreateMap<Domain.BoughtOutFinishDetail, BoughtOutFinishDetailListVM>()
               .ForMember(s => s.BoughtOutFinishDetailId, s => s.MapFrom(src => src.Id))
               .ForMember(s => s.BoughtOutFinishMadeType, s => s.MapFrom(src => src.BoughtOutFinishMadeType))
               .ForMember(s => s.PartNo, s => s.MapFrom(src => src.PartNo))
               .ForMember(s => s.SupplierPartNo, s => s.MapFrom(src => src.SupplierPartNo))
               .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDescription))
               .ForMember(s => s.AdditionalInformation, s => s.MapFrom(src => src.AdditionalInformation))
               .ForMember(s => s.Status, s => s.MapFrom(src => src.Status))
               .ForMember(s => s.StatusChangeReason, s => s.MapFrom(src => src.StatusChangeReason))
               .ForMember(s => s.RevNo, s => s.MapFrom(src => src.RevNo))
               .ForMember(s => s.RevDate, s => s.MapFrom(src => src.RevDate))
               .ForMember(s => s.PurchaseDetail, s => s.MapFrom(src => src.PurchaseDetail))
               .ForMember(s => s.Supplier, s => s.MapFrom(src => src.Supplier))
               .ForMember(s => s.PurSupplierPartNo, s => s.MapFrom(src => src.PurSupplierPartNo))
               .ForMember(s => s.LeadTimeInDays, s => s.MapFrom(src => src.LeadTimeInDays))
               .ForMember(s => s.MinOrderQuantity, s => s.MapFrom(src => src.MinOrderQuantity))
               .ForMember(s => s.Price, s => s.MapFrom(src => src.Price))
               .ForMember(s => s.ShareOfBusiness, s => s.MapFrom(src => src.ShareOfBusiness))
               .ForMember(s => s.PurAdditionalInformation, s => s.MapFrom(src => src.PurAdditionalInformation));


            CreateMap<ManufacturedPartNoDetailVM, Domain.MPMakeFrom>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.MPMakeFromId))
                .ForMember(s => s.PartMadeFrom, s => s.MapFrom(src => src.PartMadeFrom))
                .ForMember(s => s.InputPartNo, s => s.MapFrom(src => src.InputPartNo))
                .ForMember(s => s.InputWeight, s => s.MapFrom(src => src.InputWeight))
                .ForMember(s => s.Scrapgenerated, s => s.MapFrom(src => src.ScrapGenerated))
                .ForMember(s => s.QuantityPerInput, s => s.MapFrom(src => src.QuantityPerInput))
                .ForMember(s => s.YieldNotes, s => s.MapFrom(src => src.YieldNotes))
                .ForMember(s => s.PreferedRawMaterial, s => s.MapFrom(src => src.PreferedRawMaterial));

            CreateMap<Domain.MPMakeFrom, MPMakeFromListVM>()
                .ForMember(s => s.MPMakeFromId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.PartMadeFrom, s => s.MapFrom(src => src.PartMadeFrom))
                .ForMember(s => s.InputPartNo, s => s.MapFrom(src => src.InputPartNo))
                .ForMember(s => s.InputWeight, s => s.MapFrom(src => src.InputWeight))
                .ForMember(s => s.Scrapgenerated, s => s.MapFrom(src => src.Scrapgenerated))
                .ForMember(s => s.QuantityPerInput, s => s.MapFrom(src => src.QuantityPerInput))
                .ForMember(s => s.YieldNotes, s => s.MapFrom(src => src.YieldNotes))
                .ForMember(s => s.PreferedRawMaterial, s => s.MapFrom(src => src.PreferedRawMaterial));


            CreateMap<ManufacturedPartNoDetailVM, Domain.MPBOM>()
                .ForMember(s => s.Id, s => s.MapFrom(src => src.MPBOMId))
                .ForMember(s => s.PartNumber, s => s.MapFrom(src => src.PartNumber))
                .ForMember(s => s.PartDesc, s => s.MapFrom(src => src.PartDescription))
                .ForMember(s => s.Quantity, s => s.MapFrom(src => src.Quantity));

            CreateMap<Domain.MPBOM, ManufacturedPartNoDetailListVM>()
                .ForMember(s => s.MPBOMId, s => s.MapFrom(src => src.Id))
                .ForMember(s => s.PartNumber, s => s.MapFrom(src => src.PartNumber))
                .ForMember(s => s.PartDescription, s => s.MapFrom(src => src.PartDesc))
                .ForMember(s => s.Quantity, s => s.MapFrom(src => src.Quantity));

            CreateMap<Domain.UOM, UOMVM>()
                .ForMember(s => s.Name, s => s.MapFrom(src => src.Name))
                .ForMember(s => s.Id, s => s.MapFrom(src => src.Id));

            CreateMap<UOMVM, Domain.UOM>()
                .ForMember(s => s.Name, s => s.MapFrom(src => src.Name))
                .ForMember(s => s.Id, s => s.MapFrom(src => src.Id));

            
            
        }
    }
}