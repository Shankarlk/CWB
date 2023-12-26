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
                .ForMember(m => m.Id, m => m.MapFrom(src => src.DocumentTypeId));
            CreateMap<Domain.DocumentType, DocumentTypeVM>()
                .ForMember(m => m.DocumentTypeId, m => m.MapFrom(src => src.Id));
            CreateMap<Domain.DocumentType, DocumentTypeListVM>()
                .ForMember(m => m.DocumentTypeId, m => m.MapFrom(src => src.Id));

            CreateMap<PlantVM, Plant>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.PlantId));
            CreateMap<Plant, PlantVM>()
                .ForMember(m => m.PlantId, m => m.MapFrom(src => src.Id));
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
