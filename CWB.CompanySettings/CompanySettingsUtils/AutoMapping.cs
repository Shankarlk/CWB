using AutoMapper;
using CWB.CompanySettings.Domain;
using CWB.CompanySettings.ViewModels.Designations;
using CWB.CompanySettings.ViewModels.DocType;
using CWB.CompanySettings.ViewModels.EmployeeMaster;
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
                .ForMember(m => m.City, m => m.MapFrom(src => src.City))
                .ForMember(m => m.Pincode, m => m.MapFrom(src => src.Pincode))
                .ForMember(m => m.Country, m => m.MapFrom(src => src.Country))
                .ForMember(m => m.GstNo, m => m.MapFrom(src => src.GstNo))
                .ForMember(m => m.PanNo, m => m.MapFrom(src => src.PanNo))
                .ForMember(m => m.IsMainPlant, m => m.MapFrom(src => src.IsMainPlant))
                .ForMember(m => m.Change_flag, m => m.MapFrom(src => src.Change_flag))
                .ForMember(m => m.IsProductDesigned, m => m.MapFrom(src => src.IsProductDesigned));
            CreateMap<Plant, PlantVM>()
                 .ForMember(m => m.PlantId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.Address, m => m.MapFrom(src => src.Address))
                .ForMember(m => m.Notes, m => m.MapFrom(src => src.Notes))
                .ForMember(m => m.City, m => m.MapFrom(src => src.City))
                .ForMember(m => m.Pincode, m => m.MapFrom(src => src.Pincode))
                .ForMember(m => m.Country, m => m.MapFrom(src => src.Country))
                .ForMember(m => m.GstNo, m => m.MapFrom(src => src.GstNo))
                .ForMember(m => m.PanNo, m => m.MapFrom(src => src.PanNo))
                .ForMember(m => m.IsMainPlant, m => m.MapFrom(src => src.IsMainPlant))
                .ForMember(m => m.Change_flag, m => m.MapFrom(src => src.Change_flag))
                .ForMember(m => m.IsProductDesigned, m => m.MapFrom(src => src.IsProductDesigned));



            CreateMap<CityVM, City>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.CityId))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Domain.City, CityVM>()
             .ForMember(m => m.CityId, m => m.MapFrom(src => src.Id))
             .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
             .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<TimeSlotDurationVM, TimeSlotDuration>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.TimeSlotDurationId))
                .ForMember(m => m.Duration, m => m.MapFrom(src => src.Duration));
            CreateMap<TimeSlotDuration, TimeSlotDurationVM>()
             .ForMember(m => m.TimeSlotDurationId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.Duration, m => m.MapFrom(src => src.Duration));

            CreateMap<NoOfDaysTimeSlotVM, NoOfDaysTimeSlot>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.NoOfDaysTimeSlotId))
                .ForMember(m => m.Days, m => m.MapFrom(src => src.Days));
            CreateMap<NoOfDaysTimeSlot, NoOfDaysTimeSlotVM>()
             .ForMember(m => m.NoOfDaysTimeSlotId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.Days, m => m.MapFrom(src => src.Days));

            CreateMap<CountryVM, Country>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.CountryId))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Domain.Country, CountryVM>()
             .ForMember(m => m.CountryId, m => m.MapFrom(src => src.Id))
             .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
             .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            //Designation
            CreateMap<DesignationVM, Designation>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.DesignationId))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Domain.Designation, DesignationVM>()
             .ForMember(m => m.DesignationId, m => m.MapFrom(src => src.Id))
             .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
             .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Designation, DesignationListVM>()
                .ForMember(m => m.DesignationId, m => m.MapFrom(src => src.Id));

            CreateMap<ShopDepartment, ShopDepartmentVM>()
                .ForMember(m => m.DepartmentId, m => m.MapFrom(src => src.Id))
                .ForMember(m => m.PlantId, m => m.MapFrom(src => src.PlantId))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.NoOfShifts, m => m.MapFrom(src => src.NoOfShifts))
                .ForMember(m => m.Activity, m => m.MapFrom(src => src.Activity))
                .ForMember(m => m.PlantName, m => m.MapFrom(src => src.Plant.Name))
                .ForMember(m => m.Section, m => m.MapFrom(src => src.Section))
                .ForMember(m => m.ProdDept, m => m.MapFrom(src => src.ProdDept));
            CreateMap<ShopDepartmentVM, ShopDepartment>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.DepartmentId))
                .ForMember(m => m.PlantId, m => m.MapFrom(src => src.PlantId))
                .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
                .ForMember(m => m.NoOfShifts, m => m.MapFrom(src => src.NoOfShifts))
                .ForMember(m => m.Activity, m => m.MapFrom(src => src.Activity))
                .ForMember(m => m.Section, m => m.MapFrom(src => src.Section))
                .ForMember(m => m.ProdDept, m => m.MapFrom(src => src.ProdDept));

            CreateMap<HolidayVM, Holiday>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.HolidayId))
              .ForMember(m => m.PlantId, m => m.MapFrom(src => src.PlantId))
              .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
              .ForMember(m => m.HolidayDate, m => m.MapFrom(src => src.HolidayDate));

            CreateMap<PlantWorkingDetailsVM,PlantWorkingDetails>()
                .ForMember(m => m.Id, m => m.MapFrom(src => src.WDId))
               .ForMember(m => m.PlantId, m => m.MapFrom(src => src.PlantId))
               .ForMember(m => m.WeeklyOff1, m => m.MapFrom(src => src.WeeklyOff1))
               .ForMember(m => m.WeeklyOff2, m => m.MapFrom(src => src.WeeklyOff2))
               .ForMember(m => m.NoOfWeeklyOff, m => m.MapFrom(src => src.NoOfWeeklyOff))
               .ForMember(m => m.NoOfShifts, m => m.MapFrom(src => src.NoOfShifts))
               .ForMember(m => m.FirstShiftStartTime, m => m.MapFrom(src => src.FirstShiftStartTime))
               .ForMember(m => m.SecondShiftStartTime, m => m.MapFrom(src => src.SecondShiftStartTime))
               .ForMember(m => m.ThirdShiftStartTime, m => m.MapFrom(src => src.ThirdShiftStartTime))
               .ForMember(m => m.FirstShiftDuration, m => m.MapFrom(src => src.FirstShiftDuration))
               .ForMember(m => m.SecondShiftDuration, m => m.MapFrom(src => src.SecondShiftDuration))
               .ForMember(m => m.ThirdShiftDuration, m => m.MapFrom(src => src.ThirdShiftDuration))
            .ForMember(s => s.Timeslot_duration, s => s.MapFrom(src => src.Timeslot_duration))
            .ForMember(s => s.No_of_span_days, s => s.MapFrom(src => src.No_of_span_days))
            .ForMember(s => s.Retention_Days, s => s.MapFrom(src => src.Retention_Days))
            .ForMember(s => s.First_Shift_Break_start_time, s => s.MapFrom(src => src.First_Shift_Break_start_time))
            .ForMember(s => s.First_Shift_Break_duration, s => s.MapFrom(src => src.First_Shift_Break_duration))
            .ForMember(s => s.Third_Shift_Break_start_time, s => s.MapFrom(src => src.Third_Shift_Break_start_time))
            .ForMember(s => s.Third_Shift_Break_duration, s => s.MapFrom(src => src.Third_Shift_Break_duration))
            .ForMember(s => s.Sec_Shift_Break_start_time, s => s.MapFrom(src => src.Sec_Shift_Break_start_time))
            .ForMember(s => s.Sec_Shift_Break_duration, s => s.MapFrom(src => src.Sec_Shift_Break_duration))
            .ForMember(s => s.Change_flag, s => s.MapFrom(src => src.Change_flag));

            CreateMap<Holiday, HolidayVM>()
                .ForMember(m => m.HolidayId, m => m.MapFrom(src => src.Id))
               .ForMember(m => m.PlantId, m => m.MapFrom(src => src.PlantId))
               .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
               .ForMember(m => m.HolidayDate, m => m.MapFrom(src => src.HolidayDate));

            CreateMap<PlantWorkingDetails,PlantWorkingDetailsVM>()
                .ForMember(m => m.WDId, m => m.MapFrom(src => src.Id))
               .ForMember(m => m.PlantId, m => m.MapFrom(src => src.PlantId))
               .ForMember(m => m.WeeklyOff1, m => m.MapFrom(src => src.WeeklyOff1))
               .ForMember(m => m.WeeklyOff2, m => m.MapFrom(src => src.WeeklyOff2))
               .ForMember(m => m.NoOfShifts, m => m.MapFrom(src => src.NoOfShifts))
               .ForMember(m => m.NoOfWeeklyOff, m => m.MapFrom(src => src.NoOfWeeklyOff))
               .ForMember(m => m.FirstShiftStartTime, m => m.MapFrom(src => src.FirstShiftStartTime))
               .ForMember(m => m.SecondShiftStartTime, m => m.MapFrom(src => src.SecondShiftStartTime))
               .ForMember(m => m.ThirdShiftStartTime, m => m.MapFrom(src => src.ThirdShiftStartTime))
               .ForMember(m => m.FirstShiftDuration, m => m.MapFrom(src => src.FirstShiftDuration))
               .ForMember(m => m.SecondShiftDuration, m => m.MapFrom(src => src.SecondShiftDuration))
               .ForMember(m => m.ThirdShiftDuration, m => m.MapFrom(src => src.ThirdShiftDuration))
            .ForMember(s => s.Timeslot_duration, s => s.MapFrom(src => src.Timeslot_duration))
            .ForMember(s => s.No_of_span_days, s => s.MapFrom(src => src.No_of_span_days))
            .ForMember(s => s.Retention_Days, s => s.MapFrom(src => src.Retention_Days))
            .ForMember(s => s.First_Shift_Break_start_time, s => s.MapFrom(src => src.First_Shift_Break_start_time))
            .ForMember(s => s.First_Shift_Break_duration, s => s.MapFrom(src => src.First_Shift_Break_duration))
            .ForMember(s => s.Third_Shift_Break_start_time, s => s.MapFrom(src => src.Third_Shift_Break_start_time))
            .ForMember(s => s.Third_Shift_Break_duration, s => s.MapFrom(src => src.Third_Shift_Break_duration))
            .ForMember(s => s.Sec_Shift_Break_start_time, s => s.MapFrom(src => src.Sec_Shift_Break_start_time))
            .ForMember(s => s.Sec_Shift_Break_duration, s => s.MapFrom(src => src.Sec_Shift_Break_duration))
            .ForMember(s => s.Change_flag, s => s.MapFrom(src => src.Change_flag));

            CreateMap<PlantWorkingDetails, PlantVM>()
          .ForMember(m => m.WDId, m => m.MapFrom(src => src.Id))
          .ForMember(m => m.PlantId, m => m.MapFrom(src => src.Plant.Id))
          .ForMember(m => m.Name, m => m.MapFrom(src => src.Plant.Name))
          .ForMember(m => m.IsMainPlant, m => m.MapFrom(src => src.Plant.IsMainPlant))
          .ForMember(m => m.IsProductDesigned, m => m.MapFrom(src => src.Plant.IsProductDesigned))
          .ForMember(m => m.Address, m => m.MapFrom(src => src.Plant.Address))
          .ForMember(m => m.Notes, m => m.MapFrom(src => src.Plant.Notes))
          .ForMember(m => m.WeeklyOff1, m => m.MapFrom(src => src.WeeklyOff1))
          .ForMember(m => m.NoOfShifts, m => m.MapFrom(src => src.NoOfShifts))
          .ForMember(m => m.FirstShiftStartTime, m => m.MapFrom(src => src.FirstShiftStartTime))
            .ForMember(s => s.Change_flag, s => s.MapFrom(src => src.Change_flag));

            CreateMap<PlantVM, PlantWorkingDetails>()
                 .ForMember(m => m.Id, m => m.MapFrom(src => src.WDId))
                 .ForMember(m=>m.PlantId,m=>m.MapFrom(src=>src.PlantId))
                 .ForMember(m => m.NoOfShifts, m => m.MapFrom(src => src.NoOfShifts))
                 .ForMember(m => m.WeeklyOff1, m => m.MapFrom(src => src.WeeklyOff1))
                 .ForMember(m => m.FirstShiftStartTime, m => m.MapFrom(src => src.FirstShiftStartTime));


            CreateMap<UiListVM, UiList>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.UiListId))
              .ForMember(m => m.MenuLevelId, m => m.MapFrom(src => src.MenuLevelId))
              .ForMember(m => m.TopLevel, m => m.MapFrom(src => src.TopLevelId))
              .ForMember(m => m.UI_Type, m => m.MapFrom(src => src.UI_Type))
              .ForMember(m => m.UI_Name_Label, m => m.MapFrom(src => src.UI_Name_Label))
              .ForMember(m => m.UI_Part_linked_to, m => m.MapFrom(src => src.UI_Part_linked_to))
              .ForMember(m => m.Approval_Allowed, m => m.MapFrom(src => src.Approval_Allowed))
              .ForMember(m => m.View_Allowed, m => m.MapFrom(src => src.View_Allowed))
              .ForMember(m => m.Add_Edit_Allowed, m => m.MapFrom(src => src.Add_Edit_Allowed))
              .ForMember(m => m.Delete_Allowed, m => m.MapFrom(src => src.Delete_Allowed))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<UiList, UiListVM>()
               .ForMember(m => m.UiListId, m => m.MapFrom(src => src.Id))
              .ForMember(m => m.MenuLevelId, m => m.MapFrom(src => src.MenuLevelId))
              .ForMember(m => m.TopLevelId, m => m.MapFrom(src => src.TopLevel))
              .ForMember(m => m.UI_Type, m => m.MapFrom(src => src.UI_Type))
              .ForMember(m => m.UI_Name_Label, m => m.MapFrom(src => src.UI_Name_Label))
              .ForMember(m => m.UI_Part_linked_to, m => m.MapFrom(src => src.UI_Part_linked_to))
              .ForMember(m => m.Approval_Allowed, m => m.MapFrom(src => src.Approval_Allowed))
              .ForMember(m => m.View_Allowed, m => m.MapFrom(src => src.View_Allowed))
              .ForMember(m => m.Add_Edit_Allowed, m => m.MapFrom(src => src.Add_Edit_Allowed))
              .ForMember(m => m.Delete_Allowed, m => m.MapFrom(src => src.Delete_Allowed))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Org_ChartVM, Org_Chart>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Org_ChartId))
              .ForMember(m => m.First_node, m => m.MapFrom(src => src.First_node))
              .ForMember(m => m.Role_Name, m => m.MapFrom(src => src.Role_NameId))
              .ForMember(m => m.Dept_ID, m => m.MapFrom(src => src.Dept_ID))
              .ForMember(m => m.location_id, m => m.MapFrom(src => src.Location_id))
              .ForMember(m => m.Reporting_to, m => m.MapFrom(src => src.Reporting_to))
              .ForMember(m => m.Employee_Id, m => m.MapFrom(src => src.Employee_Id))
              .ForMember(m => m.Level_No, m => m.MapFrom(src => src.Level_No))
              .ForMember(m => m.Self_Comp_Id, m => m.MapFrom(src => src.Self_Comp_Id))
              .ForMember(m => m.Admin_Flag, m => m.MapFrom(src => src.Admin_Flag))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Org_Chart, Org_ChartVM>()
               .ForMember(m => m.Org_ChartId, m => m.MapFrom(src => src.Id))
              .ForMember(m => m.First_node, m => m.MapFrom(src => src.First_node))
              .ForMember(m => m.Role_NameId, m => m.MapFrom(src => src.Role_Name))
              .ForMember(m => m.Dept_ID, m => m.MapFrom(src => src.Dept_ID))
              .ForMember(m => m.Location_id, m => m.MapFrom(src => src.location_id))
              .ForMember(m => m.Reporting_to, m => m.MapFrom(src => src.Reporting_to))
              .ForMember(m => m.Employee_Id, m => m.MapFrom(src => src.Employee_Id))
              .ForMember(m => m.Level_No, m => m.MapFrom(src => src.Level_No))
              .ForMember(m => m.Self_Comp_Id, m => m.MapFrom(src => src.Self_Comp_Id))
              .ForMember(m => m.Admin_Flag, m => m.MapFrom(src => src.Admin_Flag))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Role_ListVM, Role_List>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Role_ListId))
              .ForMember(m => m.Role_Desc, m => m.MapFrom(src => src.Role_Desc))
              .ForMember(m => m.Work_Done, m => m.MapFrom(src => src.Work_Done))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Role_List, Role_ListVM>()
               .ForMember(m => m.Role_ListId, m => m.MapFrom(src => src.Id))
              .ForMember(m => m.Role_Desc, m => m.MapFrom(src => src.Role_Desc))
              .ForMember(m => m.Work_Done, m => m.MapFrom(src => src.Work_Done))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<CWB.CompanySettings.ViewModels.Location.SectionsVM, CWB.CompanySettings.Domain.Section>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.SectionsId))
              .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
              .ForMember(m => m.ShopDepartmentId, m => m.MapFrom(src => src.ShopDepartmentId))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<CWB.CompanySettings.Domain.Section, CWB.CompanySettings.ViewModels.Location.SectionsVM>()
               .ForMember(m => m.SectionsId, m => m.MapFrom(src => src.Id))
              .ForMember(m => m.Name, m => m.MapFrom(src => src.Name))
              .ForMember(m => m.ShopDepartmentId, m => m.MapFrom(src => src.ShopDepartmentId))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Role_UI_ListVM, Role_Ui_List>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Role_Ui_ListId))
              .ForMember(m => m.Ui_Id, m => m.MapFrom(src => src.Ui_Id))
              .ForMember(m => m.RoleId, m => m.MapFrom(src => src.RoleId))
              .ForMember(m => m.EmployeeId, m => m.MapFrom(src => src.EmployeeId))
              .ForMember(m => m.PermissionId, m => m.MapFrom(src => src.PermissionId))
              .ForMember(m => m.Comment, m => m.MapFrom(src => src.Comment))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Role_Ui_List, Role_UI_ListVM>()
               .ForMember(m => m.Role_Ui_ListId, m => m.MapFrom(src => src.Id))
              .ForMember(m => m.Ui_Id, m => m.MapFrom(src => src.Ui_Id))
              .ForMember(m => m.RoleId, m => m.MapFrom(src => src.RoleId))
              .ForMember(m => m.EmployeeId, m => m.MapFrom(src => src.EmployeeId))
              .ForMember(m => m.PermissionId, m => m.MapFrom(src => src.PermissionId))
              .ForMember(m => m.Comment, m => m.MapFrom(src => src.Comment))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Empl_Role_ListVM, Empl_Role_List>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Empl_Role_ListId))
              .ForMember(m => m.Ui_Id, m => m.MapFrom(src => src.Ui_Id))
              .ForMember(m => m.EmployeeId, m => m.MapFrom(src => src.EmployeeId))
              .ForMember(m => m.PermissionId, m => m.MapFrom(src => src.PermissionId))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Empl_Role_List, Empl_Role_ListVM>()
               .ForMember(m => m.Empl_Role_ListId, m => m.MapFrom(src => src.Id))
              .ForMember(m => m.Ui_Id, m => m.MapFrom(src => src.Ui_Id))
              .ForMember(m => m.EmployeeId, m => m.MapFrom(src => src.EmployeeId))
              .ForMember(m => m.PermissionId, m => m.MapFrom(src => src.PermissionId))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Permission_ListVM, Permission_List>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.PermissionId))
              .ForMember(m => m.Permission, m => m.MapFrom(src => src.PermissionId))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Permission_List, Permission_ListVM>()
               .ForMember(m => m.PermissionId, m => m.MapFrom(src => src.Id))
              .ForMember(m => m.Permission, m => m.MapFrom(src => src.Permission))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<EmployeeVM, Employee>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Employee_ID))
              .ForMember(m => m.Employee_name, m => m.MapFrom(src => src.Employee_name))
              .ForMember(m => m.Designation_Id, m => m.MapFrom(src => src.Designation_Id))
              .ForMember(m => m.Employee_No, m => m.MapFrom(src => src.Employee_No))
              .ForMember(m => m.Date_Of_Joining, m => m.MapFrom(src => src.Date_Of_Joining))
              .ForMember(m => m.Phone, m => m.MapFrom(src => src.Phone))
              .ForMember(m => m.RoleIds, m => m.MapFrom(src => src.RoleIds))
              .ForMember(m => m.HeadOfDepartment, m => m.MapFrom(src => src.HeadOfDepartment))
              .ForMember(m => m.RoleReportTo, m => m.MapFrom(src => src.RoleReportTo))
              .ForMember(m => m.Email, m => m.MapFrom(src => src.Email))
              .ForMember(m => m.UserName, m => m.MapFrom(src => src.UserName))
              .ForMember(m => m.Password, m => m.MapFrom(src => src.Password))
              .ForMember(m => m.Residential_Address, m => m.MapFrom(src => src.Residential_Address))
              .ForMember(m => m.Emerg_Contact_Name, m => m.MapFrom(src => src.Emerg_Contact_Name))
              .ForMember(m => m.Emerg_Contact_No, m => m.MapFrom(src => src.Emerg_Contact_No))
              .ForMember(m => m.Plant_Id, m => m.MapFrom(src => src.Plant_Id))
              .ForMember(m => m.Home_Dept_Id, m => m.MapFrom(src => src.Home_Dept_Id))
              .ForMember(m => m.Employee_Resigned, m => m.MapFrom(src => src.Employee_Resigned))
              .ForMember(m => m.Date_Of_Resigning, m => m.MapFrom(src => src.Date_Of_Resigning))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
            CreateMap<Employee, EmployeeVM>()
               .ForMember(m => m.Employee_ID, m => m.MapFrom(src => src.Id))
              .ForMember(m => m.Employee_name, m => m.MapFrom(src => src.Employee_name))
              .ForMember(m => m.Designation_Id, m => m.MapFrom(src => src.Designation_Id))
              .ForMember(m => m.Employee_No, m => m.MapFrom(src => src.Employee_No))
              .ForMember(m => m.Date_Of_Joining, m => m.MapFrom(src => src.Date_Of_Joining))
              .ForMember(m => m.Phone, m => m.MapFrom(src => src.Phone))
              .ForMember(m => m.RoleIds, m => m.MapFrom(src => src.RoleIds))
              .ForMember(m => m.HeadOfDepartment, m => m.MapFrom(src => src.HeadOfDepartment))
              .ForMember(m => m.RoleReportTo, m => m.MapFrom(src => src.RoleReportTo))
              .ForMember(m => m.Email, m => m.MapFrom(src => src.Email))
              .ForMember(m => m.UserName, m => m.MapFrom(src => src.UserName))
              .ForMember(m => m.Password, m => m.MapFrom(src => src.Password))
              .ForMember(m => m.Residential_Address, m => m.MapFrom(src => src.Residential_Address))
              .ForMember(m => m.Emerg_Contact_Name, m => m.MapFrom(src => src.Emerg_Contact_Name))
              .ForMember(m => m.Emerg_Contact_No, m => m.MapFrom(src => src.Emerg_Contact_No))
              .ForMember(m => m.Plant_Id, m => m.MapFrom(src => src.Plant_Id))
              .ForMember(m => m.Home_Dept_Id, m => m.MapFrom(src => src.Home_Dept_Id))
              .ForMember(m => m.Employee_Resigned, m => m.MapFrom(src => src.Employee_Resigned))
              .ForMember(m => m.Date_Of_Resigning, m => m.MapFrom(src => src.Date_Of_Resigning))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
        }
    }
}
