using AutoMapper;
using CWB.CompanySettings.Domain;
using CWB.CompanySettings.ViewModels.Designations;
using CWB.CompanySettings.ViewModels.DocType;
using CWB.CompanySettings.ViewModels.Location;

namespace CWB.CompanySettings.CompanySettingsUtils
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<DocumentTypeVM, DocumentType>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.DocumentTypeId))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.Description, m => m.MapFrom(src => src.Description)) 
                .ForMember(m => m.Extension, m => m.MapFrom(src => src.Extension))
                .ForMember(m => m.IsUploadedByUser, m => m.MapFrom(src => src.IsUploadedByUser));
            CreateMap<Domain.DocumentType, DocumentTypeVM>()
                 .ForMember(m => m.DocumentTypeId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.Description, m => m.MapFrom(src => src.Description))
                .ForMember(m => m.Extension, m => m.MapFrom(src => src.Extension))
                .ForMember(m => m.IsUploadedByUser, m => m.MapFrom(src => src.IsUploadedByUser));

            CreateMap<PlantVM, Plant>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.PlantId))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.Address, m => m.MapFrom(src => src.Address))
                .ForMember(m => m.Notes, m => m.MapFrom(src => src.Notes))
                .ForMember(m => m.IsMainPlant, m => m.MapFrom(src => src.IsMainPlant))
                .ForMember(m => m.IsProductDesigned, m => m.MapFrom(src => src.IsProductDesigned));
            CreateMap<Plant, PlantVM>()
                 .ForMember(m => m.PlantId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.Address, m => m.MapFrom(src => src.Address))
                .ForMember(m => m.Notes, m => m.MapFrom(src => src.Notes))
                .ForMember(m => m.IsMainPlant, m => m.MapFrom(src => src.IsMainPlant))
                .ForMember(m => m.IsProductDesigned, m => m.MapFrom(src => src.IsProductDesigned));
            CreateMap<Plant, PlantListVM>()
                .ForMember(m => m.PlantId, m => m.MapFrom(src => src.Id));

            CreateMap<ShopDepartment, ShopDepartmentListVM>()
                .ForMember(m => m.DepartmentId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.PlantId, m => m.MapFrom(src => src.PlantId))
                .ForMember(m => m.PlantName, m => m.MapFrom(src => src.Plant.Name));
            CreateMap<ShopDepartment, DepartmentListWithPlantVM>()
                .ForMember(m => m.DepartmentId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.DepartmentName, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.PlantId, m => m.MapFrom(src => src.PlantId))
                .ForMember(m => m.PlantName, m => m.MapFrom(src => src.Plant.Name));
            CreateMap<ShopDepartmentVM, ShopDepartment>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.DepartmentId));

            //CreateMap<DesignationVM, Designation>()
            //               .ForMember(m => m.Id, m => m.MapFrom(src => src.DesignationId));
            //CreateMap<Domain.Designation, DesignationVM>()
            //    .ForMember(m => m.DesignationId, m => m.MapFrom(src => src.Id));
            //CreateMap<Domain.Designation, DesignationListVM>()
            //    .ForMember(m => m.DesignationId, m => m.MapFrom(src => src.Id));
        }
    }
}
