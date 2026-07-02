using AutoMapper;
using CWB.Gro.Domain;
using CWB.Gro.ViewModels;
namespace CWB.Gro.GroUtils
{
    public class AutoMapping:Profile
    {
        public AutoMapping()
        {
            CreateMap<Gro_Data, Gro_DataVM>()
               .ForMember(m => m.Gro_DataId, m => m.MapFrom(src => src.Id))
               .ForMember(m => m.int_Part_No, m => m.MapFrom(src => src.int_Part_No))
               .ForMember(m => m.CWB_Customer, m => m.MapFrom(src => src.CWB_Customer))
               .ForMember(m => m.SentDate, m => m.MapFrom(src => src.SentDate))
               .ForMember(m => m.Excutive_Name, m => m.MapFrom(src => src.Excutive_Name))
               .ForMember(m => m.Indent, m => m.MapFrom(src => src.Indent))
               .ForMember(m => m.Company_Name, m => m.MapFrom(src => src.Company_Name))
               .ForMember(m => m.Gro_Part_No, m => m.MapFrom(src => src.Gro_Part_No))
               .ForMember(m => m.Reqd_Quantity, m => m.MapFrom(src => src.Reqd_Quantity))
               .ForMember(m => m.Remarks, m => m.MapFrom(src => src.Remarks))
               .ForMember(m => m.Shipping_Address, m => m.MapFrom(src => src.Shipping_Address))
               .ForMember(m => m.Shipping_City, m => m.MapFrom(src => src.Shipping_City))
               .ForMember(m => m.Shipping_State, m => m.MapFrom(src => src.Shipping_State))
               .ForMember(m => m.Shipping_PINCODE, m => m.MapFrom(src => src.Shipping_PINCODE))
               .ForMember(m => m.Contact_Person, m => m.MapFrom(src => src.Contact_Person))
               .ForMember(m => m.Contact_Person_No, m => m.MapFrom(src => src.Contact_Person_No))
               .ForMember(m => m.Part_Not_Avl, m => m.MapFrom(src => src.Part_Not_Avl))
               .ForMember(m => m.Bal_to_Disp, m => m.MapFrom(src => src.Bal_to_Disp))
               .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Gro_DataVM, Gro_Data>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Gro_DataId))
               .ForMember(m => m.int_Part_No, m => m.MapFrom(src => src.int_Part_No))
               .ForMember(m => m.CWB_Customer, m => m.MapFrom(src => src.CWB_Customer))
               .ForMember(m => m.SentDate, m => m.MapFrom(src => src.SentDate))
               .ForMember(m => m.Excutive_Name, m => m.MapFrom(src => src.Excutive_Name))
               .ForMember(m => m.Indent, m => m.MapFrom(src => src.Indent))
               .ForMember(m => m.Company_Name, m => m.MapFrom(src => src.Company_Name))
               .ForMember(m => m.Gro_Part_No, m => m.MapFrom(src => src.Gro_Part_No))
               .ForMember(m => m.Reqd_Quantity, m => m.MapFrom(src => src.Reqd_Quantity))
               .ForMember(m => m.Remarks, m => m.MapFrom(src => src.Remarks))
               .ForMember(m => m.Shipping_Address, m => m.MapFrom(src => src.Shipping_Address))
               .ForMember(m => m.Shipping_City, m => m.MapFrom(src => src.Shipping_City))
               .ForMember(m => m.Shipping_State, m => m.MapFrom(src => src.Shipping_State))
               .ForMember(m => m.Shipping_PINCODE, m => m.MapFrom(src => src.Shipping_PINCODE))
               .ForMember(m => m.Contact_Person, m => m.MapFrom(src => src.Contact_Person))
               .ForMember(m => m.Contact_Person_No, m => m.MapFrom(src => src.Contact_Person_No))
               .ForMember(m => m.Part_Not_Avl, m => m.MapFrom(src => src.Part_Not_Avl))
               .ForMember(m => m.Bal_to_Disp, m => m.MapFrom(src => src.Bal_to_Disp))
               .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Gro_Part_List, Gro_Part_ListVM>()
              .ForMember(m => m.Gro_Part_ListId, m => m.MapFrom(src => src.Id))
              .ForMember(m => m.Gro_Part_No, m => m.MapFrom(src => src.Gro_Part_No))
              .ForMember(m => m.Part_No, m => m.MapFrom(src => src.Part_No))
              .ForMember(m => m.MRP, m => m.MapFrom(src => src.MRP))
              .ForMember(m => m.Data_Update, m => m.MapFrom(src => src.Data_Update))
              .ForMember(m => m.Update_By, m => m.MapFrom(src => src.Update_By))
              .ForMember(m => m.Part_Status, m => m.MapFrom(src => src.Part_Status))
              .ForMember(m => m.OurPrice, m => m.MapFrom(src => src.OurPrice))
              .ForMember(m => m.GSTRate, m => m.MapFrom(src => src.GSTRate))
              .ForMember(m => m.HSNCode, m => m.MapFrom(src => src.HSNCode))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Gro_Part_ListVM, Gro_Part_List>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Gro_Part_ListId))
                .ForMember(m => m.Gro_Part_No, m => m.MapFrom(src => src.Gro_Part_No))
              .ForMember(m => m.Part_No, m => m.MapFrom(src => src.Part_No))
              .ForMember(m => m.MRP, m => m.MapFrom(src => src.MRP))
              .ForMember(m => m.Data_Update, m => m.MapFrom(src => src.Data_Update))
              .ForMember(m => m.Update_By, m => m.MapFrom(src => src.Update_By))
              .ForMember(m => m.Part_Status, m => m.MapFrom(src => src.Part_Status))
              .ForMember(m => m.OurPrice, m => m.MapFrom(src => src.OurPrice))
              .ForMember(m => m.GSTRate, m => m.MapFrom(src => src.GSTRate))
              .ForMember(m => m.HSNCode, m => m.MapFrom(src => src.HSNCode))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));


            CreateMap<Gro_Disp_Header, Gro_Disp_HeaderVM>()
              .ForMember(m => m.Gro_Disp_HeaderId, m => m.MapFrom(src => src.Id))
             
              .ForMember(m => m.CWB_Customer, m => m.MapFrom(src => src.CWB_Customer))
               .ForMember(m => m.Indent, m => m.MapFrom(src => src.Indent))
              .ForMember(m => m.SentDate, m => m.MapFrom(src => src.SentDate))
              .ForMember(m => m.Shipping_Address, m => m.MapFrom(src => src.Shipping_Address))
              .ForMember(m => m.Shipping_City, m => m.MapFrom(src => src.Shipping_City))
              .ForMember(m => m.Shipping_State, m => m.MapFrom(src => src.Shipping_State))
              .ForMember(m => m.Shipping_PINCODE, m => m.MapFrom(src => src.Shipping_PINCODE))
              .ForMember(m => m.Courier_Partner, m => m.MapFrom(src => src.Courier_Partner))
              .ForMember(m => m.Dispatch_Date, m => m.MapFrom(src => src.Dispatch_Date))
              .ForMember(m => m.Status, m => m.MapFrom(src => src.Status))
              .ForMember(m => m.AWB, m => m.MapFrom(src => src.AWB))
              .ForMember(m => m.DC_Printed, m => m.MapFrom(src => src.DC_Printed))
              .ForMember(m => m.Delivered_Date, m => m.MapFrom(src => src.Delivered_Date))
              .ForMember(m => m.DC_No, m => m.MapFrom(src => src.DC_No))
              .ForMember(m => m.Inv_No, m => m.MapFrom(src => src.Inv_No))
              .ForMember(m => m.Inv_Printed, m => m.MapFrom(src => src.Inv_Printed))
              .ForMember(m => m.Inv_Uploaded, m => m.MapFrom(src => src.Inv_Uploaded))
              .ForMember(m => m.Customer_Inv_Attached, m => m.MapFrom(src => src.Customer_Inv_Attached))
              .ForMember(m => m.Dispatched, m => m.MapFrom(src => src.Dispatched))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Gro_Disp_HeaderVM, Gro_Disp_Header>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Gro_Disp_HeaderId))
                .ForMember(m => m.CWB_Customer, m => m.MapFrom(src => src.CWB_Customer))
               .ForMember(m => m.Indent, m => m.MapFrom(src => src.Indent))
              .ForMember(m => m.SentDate, m => m.MapFrom(src => src.SentDate))
              .ForMember(m => m.Shipping_Address, m => m.MapFrom(src => src.Shipping_Address))
              .ForMember(m => m.Shipping_City, m => m.MapFrom(src => src.Shipping_City))
              .ForMember(m => m.Shipping_State, m => m.MapFrom(src => src.Shipping_State))
              .ForMember(m => m.Shipping_PINCODE, m => m.MapFrom(src => src.Shipping_PINCODE))
              .ForMember(m => m.Courier_Partner, m => m.MapFrom(src => src.Courier_Partner))
              .ForMember(m => m.Dispatch_Date, m => m.MapFrom(src => src.Dispatch_Date))
              .ForMember(m => m.Status, m => m.MapFrom(src => src.Status))
              .ForMember(m => m.AWB, m => m.MapFrom(src => src.AWB))
              .ForMember(m => m.DC_Printed, m => m.MapFrom(src => src.DC_Printed))
              .ForMember(m => m.Delivered_Date, m => m.MapFrom(src => src.Delivered_Date))
              .ForMember(m => m.DC_No, m => m.MapFrom(src => src.DC_No))
              .ForMember(m => m.Inv_No, m => m.MapFrom(src => src.Inv_No))
              .ForMember(m => m.Inv_Printed, m => m.MapFrom(src => src.Inv_Printed))
              .ForMember(m => m.Inv_Uploaded, m => m.MapFrom(src => src.Inv_Uploaded))
              .ForMember(m => m.Customer_Inv_Attached, m => m.MapFrom(src => src.Customer_Inv_Attached))
              .ForMember(m => m.Dispatched, m => m.MapFrom(src => src.Dispatched))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));



            CreateMap<Gro_Disp_Det, Gro_Disp_DetVM>()
              .ForMember(m => m.Gro_Disp_DetId, m => m.MapFrom(src => src.Id))

              .ForMember(m => m.Gro_Disp_Header_ID, m => m.MapFrom(src => src.Gro_Disp_Header_ID))
               .ForMember(m => m.Gro_data_ID, m => m.MapFrom(src => src.Gro_data_ID))
              .ForMember(m => m.Gro_Part_No, m => m.MapFrom(src => src.Gro_Part_No))
              .ForMember(m => m.int_Part_No, m => m.MapFrom(src => src.int_Part_No))
              .ForMember(m => m.Qnty_Dispatched, m => m.MapFrom(src => src.Qnty_Dispatched))
              .ForMember(m => m.Qnty_Recd, m => m.MapFrom(src => src.Qnty_Recd))
              .ForMember(m => m.Indent, m => m.MapFrom(src => src.Indent))
              .ForMember(m => m.Label_print, m => m.MapFrom(src => src.Label_print))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Gro_Disp_DetVM, Gro_Disp_Det>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Gro_Disp_DetId))
                .ForMember(m => m.Gro_Disp_Header_ID, m => m.MapFrom(src => src.Gro_Disp_Header_ID))
               .ForMember(m => m.Gro_data_ID, m => m.MapFrom(src => src.Gro_data_ID))
              .ForMember(m => m.Gro_Part_No, m => m.MapFrom(src => src.Gro_Part_No))
              .ForMember(m => m.int_Part_No, m => m.MapFrom(src => src.int_Part_No))
              .ForMember(m => m.Qnty_Dispatched, m => m.MapFrom(src => src.Qnty_Dispatched))
              .ForMember(m => m.Qnty_Recd, m => m.MapFrom(src => src.Qnty_Recd))
              .ForMember(m => m.Indent, m => m.MapFrom(src => src.Indent))
              .ForMember(m => m.Label_print, m => m.MapFrom(src => src.Label_print))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));









            CreateMap<Cust_Specific_Data, Cust_Specific_DataVM>()
             .ForMember(m => m.Cust_Specific_DataId, m => m.MapFrom(src => src.Id))
             .ForMember(m => m.File_Location, m => m.MapFrom(src => src.File_Location))
              .ForMember(m => m.Last_Upload_Row_No, m => m.MapFrom(src => src.Last_Upload_Row_No))
             .ForMember(m => m.Last_Upload_date, m => m.MapFrom(src => src.Last_Upload_date))
             .ForMember(m => m.CWB_Customer, m => m.MapFrom(src => src.CWB_Customer))
             .ForMember(m => m.UI_ID, m => m.MapFrom(src => src.UI_ID))
             .ForMember(m => m.Upload_Mapped_Table, m => m.MapFrom(src => src.Upload_Mapped_Table))
             .ForMember(m => m.Disp_Head_Map_Table, m => m.MapFrom(src => src.Disp_Head_Map_Table))
             .ForMember(m => m.Disp_Det_Map_Table, m => m.MapFrom(src => src.Disp_Det_Map_Table))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Cust_Specific_DataVM, Cust_Specific_Data>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Cust_Specific_DataId))
                 .ForMember(m => m.File_Location, m => m.MapFrom(src => src.File_Location))
              .ForMember(m => m.Last_Upload_Row_No, m => m.MapFrom(src => src.Last_Upload_Row_No))
             .ForMember(m => m.Last_Upload_date, m => m.MapFrom(src => src.Last_Upload_date))
             .ForMember(m => m.CWB_Customer, m => m.MapFrom(src => src.CWB_Customer))
             .ForMember(m => m.UI_ID, m => m.MapFrom(src => src.UI_ID))
             .ForMember(m => m.Upload_Mapped_Table, m => m.MapFrom(src => src.Upload_Mapped_Table))
             .ForMember(m => m.Disp_Head_Map_Table, m => m.MapFrom(src => src.Disp_Head_Map_Table))
             .ForMember(m => m.Disp_Det_Map_Table, m => m.MapFrom(src => src.Disp_Det_Map_Table))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));



            CreateMap<Upload_Format, Upload_FormatVM>()
             .ForMember(m => m.Upload_Format_ID, m => m.MapFrom(src => src.Id))
             .ForMember(m => m.CWB_Customer, m => m.MapFrom(src => src.CWB_Customer))
              .ForMember(m => m.Column_Name, m => m.MapFrom(src => src.Column_Name))
             .ForMember(m => m.Column_Position, m => m.MapFrom(src => src.Column_Position))
             .ForMember(m => m.Field_Type, m => m.MapFrom(src => src.Field_Type))
             .ForMember(m => m.Mapped_Field, m => m.MapFrom(src => src.Mapped_Field))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Upload_FormatVM, Upload_Format>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Upload_Format_ID))
                 .ForMember(m => m.CWB_Customer, m => m.MapFrom(src => src.CWB_Customer))
              .ForMember(m => m.Column_Name, m => m.MapFrom(src => src.Column_Name))
             .ForMember(m => m.Column_Position, m => m.MapFrom(src => src.Column_Position))
             .ForMember(m => m.Field_Type, m => m.MapFrom(src => src.Field_Type))
             .ForMember(m => m.Mapped_Field, m => m.MapFrom(src => src.Mapped_Field))
              .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Field_Type, Field_TypeVM>()
            .ForMember(m => m.Field_Type_ID, m => m.MapFrom(src => src.Id))
            .ForMember(m => m.Field_Type_Desc, m => m.MapFrom(src => src.Field_Type_Desc));


            CreateMap<Field_TypeVM, Field_Type>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Field_Type_ID))
                 .ForMember(m => m.Field_Type_Desc, m => m.MapFrom(src => src.Field_Type_Desc));
            CreateMap<Courier_List, Courier_ListVM>()
          .ForMember(m => m.courier_List_ID, m => m.MapFrom(src => src.Id))
          .ForMember(m => m.Courier_Name, m => m.MapFrom(src => src.Courier_Name))
           .ForMember(m => m.Contact_Person, m => m.MapFrom(src => src.Contact_Person))
          .ForMember(m => m.Contact_Phone, m => m.MapFrom(src => src.Contact_Phone))
           .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Courier_ListVM, Courier_List>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.courier_List_ID))
                .ForMember(m => m.Courier_Name, m => m.MapFrom(src => src.Courier_Name))
           .ForMember(m => m.Contact_Person, m => m.MapFrom(src => src.Contact_Person))
          .ForMember(m => m.Contact_Phone, m => m.MapFrom(src => src.Contact_Phone))
           .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Printout_format, Printout_formatVM>()
        .ForMember(m => m.Printout_Format_ID, m => m.MapFrom(src => src.Id))
        .ForMember(m => m.CWB_Customer, m => m.MapFrom(src => src.CWB_Customer))
         .ForMember(m => m.Template_Location, m => m.MapFrom(src => src.Template_Location))
        .ForMember(m => m.Purpose, m => m.MapFrom(src => src.Purpose))
         .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Printout_formatVM, Printout_format>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Printout_Format_ID))
                .ForMember(m => m.CWB_Customer, m => m.MapFrom(src => src.CWB_Customer))
           .ForMember(m => m.Template_Location, m => m.MapFrom(src => src.Template_Location))
          .ForMember(m => m.Purpose, m => m.MapFrom(src => src.Purpose))
           .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));




            CreateMap<Gro_Stock_List, Gro_Stock_ListVM>()
        .ForMember(m => m.Gro_Stock_ListId, m => m.MapFrom(src => src.Id))
        .ForMember(m => m.Gro_Part_List_ID, m => m.MapFrom(src => src.Gro_Part_List_ID))
         .ForMember(m => m.Qnty_on_Hand, m => m.MapFrom(src => src.Qnty_on_Hand))
        .ForMember(m => m.Last_Sl_No, m => m.MapFrom(src => src.Last_Sl_No))
        .ForMember(m => m.Qnty_Correction_Date, m => m.MapFrom(src => src.Qnty_Correction_Date))
        .ForMember(m => m.Correction_User, m => m.MapFrom(src => src.Correction_User))
         .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Gro_Stock_ListVM, Gro_Stock_List>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Gro_Stock_ListId))
               .ForMember(m => m.Gro_Part_List_ID, m => m.MapFrom(src => src.Gro_Part_List_ID))
         .ForMember(m => m.Qnty_on_Hand, m => m.MapFrom(src => src.Qnty_on_Hand))
        .ForMember(m => m.Last_Sl_No, m => m.MapFrom(src => src.Last_Sl_No))
        .ForMember(m => m.Qnty_Correction_Date, m => m.MapFrom(src => src.Qnty_Correction_Date))
        .ForMember(m => m.Correction_User, m => m.MapFrom(src => src.Correction_User))
         .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<TK_DC_Inv_Contrl, TK_DC_Inv_ContrlVM>()
         .ForMember(m => m.TK_DC_Inv_ContrlId, m => m.MapFrom(src => src.Id))
         .ForMember(m => m.TK_DC_Last_No, m => m.MapFrom(src => src.TK_DC_Last_No))
          .ForMember(m => m.TK_Inv_Last_No, m => m.MapFrom(src => src.TK_Inv_Last_No))
         .ForMember(m => m.DC_Enable, m => m.MapFrom(src => src.DC_Enable))
         .ForMember(m => m.Inv_Print_Enable, m => m.MapFrom(src => src.Inv_Print_Enable))
         .ForMember(m => m.Inv_Push_Enable, m => m.MapFrom(src => src.Inv_Push_Enable))
          .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<TK_DC_Inv_ContrlVM, TK_DC_Inv_Contrl>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.TK_DC_Inv_ContrlId))
                  .ForMember(m => m.TK_DC_Last_No, m => m.MapFrom(src => src.TK_DC_Last_No))
          .ForMember(m => m.TK_Inv_Last_No, m => m.MapFrom(src => src.TK_Inv_Last_No))
         .ForMember(m => m.DC_Enable, m => m.MapFrom(src => src.DC_Enable))
         .ForMember(m => m.Inv_Print_Enable, m => m.MapFrom(src => src.Inv_Print_Enable))
         .ForMember(m => m.Inv_Push_Enable, m => m.MapFrom(src => src.Inv_Push_Enable))
         .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));


            CreateMap<Gro_Stock_Det, Gro_Stock_DetVM>()
           .ForMember(m => m.Gro_Stock_DetId, m => m.MapFrom(src => src.Id))
           .ForMember(m => m.Gro_Part_List_ID, m => m.MapFrom(src => src.Gro_Part_List_ID))
            .ForMember(m => m.Part_Sl_No, m => m.MapFrom(src => src.Part_Sl_No))
           .ForMember(m => m.Sl_No_Status_ID, m => m.MapFrom(src => src.Sl_No_Status_ID))
          
            .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Gro_Stock_DetVM, Gro_Stock_Det>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Gro_Stock_DetId))
                   .ForMember(m => m.Gro_Part_List_ID, m => m.MapFrom(src => src.Gro_Part_List_ID))
            .ForMember(m => m.Part_Sl_No, m => m.MapFrom(src => src.Part_Sl_No))
           .ForMember(m => m.Sl_No_Status_ID, m => m.MapFrom(src => src.Sl_No_Status_ID))
         .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Sl_No_Status_List, Sl_No_Status_ListVM>()
           .ForMember(m => m.Sl_No_Status_ID, m => m.MapFrom(src => src.Id))
           .ForMember(m => m.Sl_No_Status_Desc, m => m.MapFrom(src => src.Sl_No_Status_Desc));


            CreateMap<Sl_No_Status_ListVM, Sl_No_Status_List>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Sl_No_Status_ID))
                 .ForMember(m => m.Sl_No_Status_Desc, m => m.MapFrom(src => src.Sl_No_Status_Desc));





            CreateMap<Indent_Part_Sl_No, Indent_Part_Sl_NoVM>()
        .ForMember(m => m.Indent_Part_Sl_No_ID, m => m.MapFrom(src => src.Id))
        .ForMember(m => m.Gro_Disp_Det_ID, m => m.MapFrom(src => src.Gro_Disp_Det_ID))
         .ForMember(m => m.Gro_Stock_Det_ID, m => m.MapFrom(src => src.Gro_Stock_Det_ID))
          .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Indent_Part_Sl_NoVM, Indent_Part_Sl_No>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Indent_Part_Sl_No_ID))
                .ForMember(m => m.Gro_Disp_Det_ID, m => m.MapFrom(src => src.Gro_Disp_Det_ID))
           .ForMember(m => m.Gro_Stock_Det_ID, m => m.MapFrom(src => src.Gro_Stock_Det_ID))
           .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));




            CreateMap<Gro_Indent_DispHead, Gro_Indent_DispHeadVM>()
    .ForMember(m => m.Gro_Indent_DispHead_ID, m => m.MapFrom(src => src.Id))
    .ForMember(m => m.Gro_Indent, m => m.MapFrom(src => src.Gro_Indent))
     .ForMember(m => m.Gro_Disp_Header_ID, m => m.MapFrom(src => src.Gro_Disp_Header_ID))
      .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));

            CreateMap<Gro_Indent_DispHeadVM, Gro_Indent_DispHead>()
               .ForMember(m => m.Id, m => m.MapFrom(src => src.Gro_Indent_DispHead_ID))
                .ForMember(m => m.Gro_Indent, m => m.MapFrom(src => src.Gro_Indent))
           .ForMember(m => m.Gro_Disp_Header_ID, m => m.MapFrom(src => src.Gro_Disp_Header_ID))
           .ForMember(m => m.TenantId, m => m.MapFrom(src => src.TenantId));
        }
    }
}
