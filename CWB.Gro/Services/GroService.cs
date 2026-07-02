using AutoMapper;
using CWB.Logging;
using CWB.Gro.Domain;
using CWB.Gro.Infrastructure;
using CWB.Gro.Repositories;
using CWB.Gro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Gro.Services
{
    public class GroService:IGroService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGro_DataRepository _gro_DataRepository;
        private readonly IGro_Part_ListRepository _gro_part_listRepository;
        private readonly IGro_Disp_HeaderRepository _gro_Disp_HeaderRepository;
        private readonly ICust_Specific_DataRepository _Cust_Specific_DataRepository;
        private readonly IGro_Disp_DetRepository _gro_Disp_DetRepository;
        private readonly IUpload_FormatRepository _upload_FormatRepository;
        private readonly IField_TypeRepository _FieldTypeRepository;
        private readonly ICourier_ListRepository _CourierlistRepository;
        private readonly IPrintout_formatRepository _PrintotformatRepository;
        private readonly IGro_Stock_ListRepository _GroStocklistRepository;
        private readonly ITK_DC_Inv_ContrlRepository _tK_DC_Inv_ContrlRepository;
        private readonly IGro_Stock_DetRepository _gro_Stock_DetRepository;
        private readonly ISl_No_Status_ListRepository _slNo_Status_ListRepository;
        private readonly IIndent_Part_Sl_NoRepository _indent_Part_Sl_NoRepository;
        private readonly IGro_Indent_DispHeadRepository _Gro_Indent_DispHeadRepository;
        public GroService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork, IGro_DataRepository gro_DataRepository,IGro_Part_ListRepository  gro_part_listRepository,
            IGro_Disp_HeaderRepository gro_Disp_HeaderRepository, ICust_Specific_DataRepository Cust_Specific_DataRepository, IGro_Disp_DetRepository gro_Disp_DetRepository,
            IUpload_FormatRepository upload_FormatRepository, IField_TypeRepository FieldTypeRepository,ICourier_ListRepository courier_ListRepository,
            IPrintout_formatRepository printout_FormatRepository, IGro_Stock_ListRepository GroStocklistRepository, ITK_DC_Inv_ContrlRepository tK_DC_Inv_ContrlRepository,
            IGro_Stock_DetRepository gro_Stock_DetRepository, ISl_No_Status_ListRepository slNo_Status_ListRepository, IIndent_Part_Sl_NoRepository indent_Part_Sl_NoRepository,
            IGro_Indent_DispHeadRepository Gro_Indent_DispHeadRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _gro_DataRepository = gro_DataRepository;
            _gro_part_listRepository = gro_part_listRepository;
            _gro_Disp_HeaderRepository = gro_Disp_HeaderRepository;
            _Cust_Specific_DataRepository = Cust_Specific_DataRepository;
            _gro_Disp_DetRepository = gro_Disp_DetRepository;
            _upload_FormatRepository = upload_FormatRepository;
            _FieldTypeRepository = FieldTypeRepository;
            _CourierlistRepository = courier_ListRepository;
            _PrintotformatRepository = printout_FormatRepository;
            _GroStocklistRepository = GroStocklistRepository;
            _tK_DC_Inv_ContrlRepository = tK_DC_Inv_ContrlRepository;
            _gro_Stock_DetRepository = gro_Stock_DetRepository;
            _slNo_Status_ListRepository = slNo_Status_ListRepository;
            _indent_Part_Sl_NoRepository = indent_Part_Sl_NoRepository;
            _Gro_Indent_DispHeadRepository = Gro_Indent_DispHeadRepository;
        }
        public async Task<IEnumerable<Gro_DataVM>> AllGroData(long tenantId)
        {
            var allwo = _gro_DataRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Gro_DataVM>>(allwo);
        }
        public async Task<List<Gro_DataVM>> MultipleGrodata(List<Gro_DataVM> GroDataVM)
        {
            foreach (Gro_DataVM item in GroDataVM)
            {
                var gro = _mapper.Map<Gro_Data>(item);
                
                    if (gro.Id == 0)
                    {
                    
                        try
                        {
                            await _gro_DataRepository.AddAsync(gro);
                            await _unitOfWork.CommitAsync();
                            
                        }
                        catch (Exception ex)
                        {
                            Exception exa = ex.InnerException;
                            string msg = ex.Message;
                        }
                    }
                    else
                    {

                    }
                    try
                    {
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
           
                item.Gro_DataId = gro.Id;
                
            }
            return GroDataVM;
        }
        public async Task<Gro_DataVM> PostGrodata(Gro_DataVM GroDataVM)
        {
            
                var gro = _mapper.Map<Gro_Data>(GroDataVM);

                if (gro.Id == 0)
                {

                    try
                    {
                        await _gro_DataRepository.AddAsync(gro);
                        await _unitOfWork.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

            GroDataVM.Gro_DataId = gro.Id;

             
            return GroDataVM;
        }
        public async Task<Gro_DataVM> UpdateGrodataBaltoDispatch(Gro_DataVM GroDataVM)
        {

            var gro = _mapper.Map<Gro_Data>(GroDataVM);

            if (gro.Id > 0)
            {

                gro = await _gro_DataRepository.SingleOrDefaultAsync(x => x.Id == gro.Id);
                gro.Bal_to_Disp = GroDataVM.Bal_to_Disp;
                gro = await _gro_DataRepository.UpdateAsync(gro.Id, gro);

            }
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Gro_DataId = gro.Id;


            return GroDataVM;
        }


        public async Task<bool> DeleteGroData(long Id)
        {
            var co = await _gro_DataRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _gro_DataRepository.Remove(co);
                    await _unitOfWork.CommitAsync();
                   
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }



        public async Task<IEnumerable<Gro_Part_ListVM>> AllGroPartList(long tenantId)
        {
            var allwo = _gro_part_listRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Gro_Part_ListVM>>(allwo);
        }
        public async Task<List<Gro_Part_ListVM>> MultipleGroPartList(List<Gro_Part_ListVM> GroDataVM)
        {
            foreach (Gro_Part_ListVM item in GroDataVM)
            {
                var gro = _mapper.Map<Gro_Part_List>(item);

                if (gro.Id == 0)
                {

                    try
                    {
                        await _gro_part_listRepository.AddAsync(gro);
                        await _unitOfWork.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.Gro_Part_ListId = gro.Id;

            }
            return GroDataVM;
        }
        public async Task<Gro_Part_ListVM> PostGroPartList(Gro_Part_ListVM GroDataVM)
        {

            var gro = _mapper.Map<Gro_Part_List>(GroDataVM);

            if (gro.Id == 0)
            {

                try
                {
                    await _gro_part_listRepository.AddAsync(gro);
                   // await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                gro = await _gro_part_listRepository.SingleOrDefaultAsync(x => x.Id == gro.Id);
                gro.Gro_Part_No = GroDataVM.Gro_Part_No;
                gro.MRP = GroDataVM.MRP;
                gro.OurPrice = GroDataVM.OurPrice;
                gro.Part_No = GroDataVM.Part_No;
                gro.HSNCode = GroDataVM.HSNCode;
                gro.GSTRate = GroDataVM.GSTRate;
                gro.Part_Status = GroDataVM.Part_Status;
                gro = await _gro_part_listRepository.UpdateAsync(gro.Id, gro);
               // await _unitOfWork.CommitAsync();
            }
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Gro_Part_ListId = gro.Id;


            return GroDataVM;
        }



        public async Task<bool> DeleteGroPartList(long Id)
        {
            var co = await _gro_part_listRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _gro_part_listRepository.Remove(co);
                    await _unitOfWork.CommitAsync();

                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }


        public async Task<IEnumerable<Gro_Disp_HeaderVM>> AllGroDispatchHeader(long tenantId)
        {
            var allwo = _gro_Disp_HeaderRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Gro_Disp_HeaderVM>>(allwo);
        }
        public async Task<List<Gro_Disp_HeaderVM>> MultipleGroDispatchHeader(List<Gro_Disp_HeaderVM> GroDataVM)
        {
            foreach (Gro_Disp_HeaderVM item in GroDataVM)
            {
                var gro = _mapper.Map<Gro_Disp_Header>(item);

                if (gro.Id == 0)
                {

                    try
                    {
                        await _gro_Disp_HeaderRepository.AddAsync(gro);
                        await _unitOfWork.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.Gro_Disp_HeaderId = gro.Id;

            }
            return GroDataVM;
        }
        public async Task<Gro_Disp_HeaderVM> PostGroDispatchHeader(Gro_Disp_HeaderVM GroDataVM)
        {

            var gro = _mapper.Map<Gro_Disp_Header>(GroDataVM);

            if (gro.Id == 0)
            {

                try
                {
                    await _gro_Disp_HeaderRepository.AddAsync(gro);
                    await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {

            }
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Gro_Disp_HeaderId = gro.Id;


            return GroDataVM;
        }

        public async Task<Gro_Disp_HeaderVM> UpdateGroDispatchHeader(Gro_Disp_HeaderVM GroDataVM)
        {

            var gro = _mapper.Map<Gro_Disp_Header>(GroDataVM);

            if (gro.Id > 0)
            {

                try
                {
                    gro = await _gro_Disp_HeaderRepository.SingleOrDefaultAsync(x => x.Id == gro.Id);
                    gro.Shipping_Address = GroDataVM.Shipping_Address;
                    gro.Shipping_City = GroDataVM.Shipping_City;
                    gro.Shipping_PINCODE = GroDataVM.Shipping_PINCODE;


                    gro = await _gro_Disp_HeaderRepository.UpdateAsync(gro.Id, gro);
                    //await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Gro_Disp_HeaderId = gro.Id;


            return GroDataVM;
        }
        public async Task<Gro_Disp_HeaderVM> UpdateGroDispatchHeaderAWB(Gro_Disp_HeaderVM GroDataVM)
        {

            var gro = _mapper.Map<Gro_Disp_Header>(GroDataVM);

            if (gro.Id > 0)
            {

                try
                {
                    gro = await _gro_Disp_HeaderRepository.SingleOrDefaultAsync(x => x.Id == gro.Id);
                    gro.Courier_Partner = GroDataVM.Courier_Partner;
                    gro.AWB = GroDataVM.AWB;
                    gro.Dispatch_Date = GroDataVM.Dispatch_Date;
                    gro.Dispatched = 'Y';

                    gro = await _gro_Disp_HeaderRepository.UpdateAsync(gro.Id, gro);
                    //await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Gro_Disp_HeaderId = gro.Id;


            return GroDataVM;
        }


        public async Task<Gro_Disp_HeaderVM> UpdateGroDispatchHeaderDeliveryDate(Gro_Disp_HeaderVM GroDataVM)
        {

            var gro = _mapper.Map<Gro_Disp_Header>(GroDataVM);

            if (gro.Id > 0)
            {

                try
                {
                    gro = await _gro_Disp_HeaderRepository.SingleOrDefaultAsync(x => x.Id == gro.Id);
                    gro.Delivered_Date = GroDataVM.Delivered_Date;
                   


                    gro = await _gro_Disp_HeaderRepository.UpdateAsync(gro.Id, gro);
                    //await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Gro_Disp_HeaderId = gro.Id;


            return GroDataVM;
        }
        public async Task<bool> DeleteGroDispatchHeader(long Id)
        {
            var co = await _gro_Disp_HeaderRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _gro_Disp_HeaderRepository.Remove(co);
                    await _unitOfWork.CommitAsync();

                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }


        public async Task<IEnumerable<Cust_Specific_DataVM>> AllCustSpecificData(long tenantId)
        {
            var allwo = _Cust_Specific_DataRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Cust_Specific_DataVM>>(allwo);
        }
        public async Task<List<Cust_Specific_DataVM>> MultipleCustSpecificData(List<Cust_Specific_DataVM> GroDataVM)
        {
            foreach (Cust_Specific_DataVM item in GroDataVM)
            {
                var gro = _mapper.Map<Cust_Specific_Data>(item);

                if (gro.Id == 0)
                {

                    try
                    {
                        await _Cust_Specific_DataRepository.AddAsync(gro);
                        await _unitOfWork.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.Cust_Specific_DataId = gro.Id;

            }
            return GroDataVM;
        }
        public async Task<Cust_Specific_DataVM> PostCustSpecificData(Cust_Specific_DataVM GroDataVM)
        {

            var gro = _mapper.Map<Cust_Specific_Data>(GroDataVM);

            if (gro.Id == 0)
            {

                try
                {
                    await _Cust_Specific_DataRepository.AddAsync(gro);
                    await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                gro.Last_Upload_Row_No = GroDataVM.Last_Upload_Row_No;
                gro.Last_Upload_date = GroDataVM.Last_Upload_date;
                gro = await _Cust_Specific_DataRepository.UpdateAsync(gro.Id, gro);

            }
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Cust_Specific_DataId = gro.Id;


            return GroDataVM;
        }



        public async Task<bool> DeleteCustSpecificData(long Id)
        {
            var co = await _Cust_Specific_DataRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _Cust_Specific_DataRepository.Remove(co);
                    await _unitOfWork.CommitAsync();

                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }




        public async Task<IEnumerable<Gro_Disp_DetVM>> AllGroDispatchDetails(long tenantId)
        {
            var allwo = _gro_Disp_DetRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Gro_Disp_DetVM>>(allwo);
        }
        public async Task<List<Gro_Disp_DetVM>> MultipleGroDispatchDetails(List<Gro_Disp_DetVM> GroDataVM)
        {
            foreach (Gro_Disp_DetVM item in GroDataVM)
            {
                var gro = _mapper.Map<Gro_Disp_Det>(item);

                if (gro.Id == 0)
                {

                    try
                    {
                        await _gro_Disp_DetRepository.AddAsync(gro);
                        await _unitOfWork.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.Gro_Disp_DetId = gro.Id;

            }
            return GroDataVM;
        }
        public async Task<Gro_Disp_DetVM> PostGroDispatchDetail(Gro_Disp_DetVM GroDataVM)
        {

            var gro = _mapper.Map<Gro_Disp_Det>(GroDataVM);

            if (gro.Id == 0)
            {

                try
                {
                    await _gro_Disp_DetRepository.AddAsync(gro);
                    await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {

            }
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Gro_Disp_DetId = gro.Id;


            return GroDataVM;
        }

        public async Task<Gro_Disp_DetVM> UpdateGroDispatchDetailQty(Gro_Disp_DetVM GroDataVM)
        {

            var gro = _mapper.Map<Gro_Disp_Det>(GroDataVM);

            if (gro.Id > 0)
            {

                try
                {
                    gro = await _gro_Disp_DetRepository.SingleOrDefaultAsync(x => x.Id == gro.Id);
                    gro.Qnty_Dispatched = GroDataVM.Qnty_Dispatched;


                    gro = await _gro_Disp_DetRepository.UpdateAsync(gro.Id, gro);
                    //await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Gro_Disp_DetId = gro.Id;


            return GroDataVM;
        }

        public async Task<bool> DeleteGroDispatchDetail(long Id)
        {
            var co = await _gro_Disp_DetRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _gro_Disp_DetRepository.Remove(co);
                    await _unitOfWork.CommitAsync();

                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }


        public async Task<IEnumerable<Upload_FormatVM>> AllUploadformats(long tenantId)
        {
            var allwo = _upload_FormatRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Upload_FormatVM>>(allwo);
        }
        public async Task<List<Upload_FormatVM>> MultipleUploadFormats(List<Upload_FormatVM> GroDataVM)
        {
            foreach (Upload_FormatVM item in GroDataVM)
            {
                var gro = _mapper.Map<Upload_Format>(item);

                if (gro.Id == 0)
                {

                    try
                    {
                        await _upload_FormatRepository.AddAsync(gro);
                        await _unitOfWork.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.Upload_Format_ID = gro.Id;

            }
            return GroDataVM;
        }
        public async Task<Upload_FormatVM> PostUploadFormat(Upload_FormatVM GroDataVM)
        {

            var gro = _mapper.Map<Upload_Format>(GroDataVM);

            if (gro.Id == 0)
            {

                try
                {
                    await _upload_FormatRepository.AddAsync(gro);
                    await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {

            }
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Upload_Format_ID = gro.Id;


            return GroDataVM;
        }



        public async Task<bool> DeleteUploadformat(long Id)
        {
            var co = await _upload_FormatRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _upload_FormatRepository.Remove(co);
                    await _unitOfWork.CommitAsync();

                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }

        public async Task<Field_TypeVM> GetFeildType(long Id)
        {
            var allpp = await _FieldTypeRepository.SingleOrDefaultAsync(d => d.Id == Id);
            if (allpp != null)
            {
                return _mapper.Map<Field_TypeVM>(allpp);
            }
            return new Field_TypeVM { Field_Type_ID = -1 };
        }

        public async Task<IEnumerable<Courier_ListVM>> AllCourierlist(long tenantId)
        {
            var allwo = _CourierlistRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Courier_ListVM>>(allwo);
        }
        public async Task<List<Courier_ListVM>> MultipleCourierList(List<Courier_ListVM> GroDataVM)
        {
            foreach (Courier_ListVM item in GroDataVM)
            {
                var gro = _mapper.Map<Courier_List>(item);

                if (gro.Id == 0)
                {

                    try
                    {
                        await _CourierlistRepository.AddAsync(gro);
                        await _unitOfWork.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.courier_List_ID  = gro.Id;

            }
            return GroDataVM;
        }
        public async Task<Courier_ListVM> PostCourierList(Courier_ListVM GroDataVM)
        {

            var gro = _mapper.Map<Courier_List>(GroDataVM);

            if (gro.Id == 0)
            {

                try
                {
                    await _CourierlistRepository.AddAsync(gro);
                    //await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                gro = await _CourierlistRepository.SingleOrDefaultAsync(x => x.Id == gro.Id);
                gro.Courier_Name = GroDataVM.Courier_Name;
                gro.Contact_Person = GroDataVM.Contact_Person;
                gro.Contact_Phone = GroDataVM.Contact_Phone;
               
              
                gro = await _CourierlistRepository.UpdateAsync(gro.Id, gro);

            }
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.courier_List_ID = gro.Id;


            return GroDataVM;
        }



        public async Task<bool> DeleteCourier(long Id)
        {
            var co = await _CourierlistRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _CourierlistRepository.Remove(co);
                    await _unitOfWork.CommitAsync();

                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }


        public async Task<IEnumerable<Printout_formatVM>> AllPrintoutformats(long tenantId)
        {
            var allwo = _PrintotformatRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Printout_formatVM>>(allwo);
        }
        public async Task<List<Printout_formatVM>> MultiplePrintoutFormat(List<Printout_formatVM> GroDataVM)
        {
            foreach (Printout_formatVM item in GroDataVM)
            {
                var gro = _mapper.Map<Printout_format>(item);

                if (gro.Id == 0)
                {

                    try
                    {
                        await _PrintotformatRepository.AddAsync(gro);
                        await _unitOfWork.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.Printout_Format_ID = gro.Id;

            }
            return GroDataVM;
        }
        public async Task<Printout_formatVM> PostPrintoutformat(Printout_formatVM GroDataVM)
        {

            var gro = _mapper.Map<Printout_format>(GroDataVM);

            if (gro.Id == 0)
            {

                try
                {
                    await _PrintotformatRepository.AddAsync(gro);
                    await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {

            }
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Printout_Format_ID = gro.Id;


            return GroDataVM;
        }



        public async Task<bool> DeletePrintoutformat(long Id)
        {
            var co = await _PrintotformatRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _PrintotformatRepository.Remove(co);
                    await _unitOfWork.CommitAsync();

                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        public async Task<IEnumerable<Gro_Stock_ListVM>> AllGroStockList(long tenantId)
        {
            var allStock =  _GroStocklistRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Gro_Stock_ListVM>>(allStock);
        }

        public async Task<List<Gro_Stock_ListVM>> MultipleGroStockList(List<Gro_Stock_ListVM> stockListVM)
        {
            foreach (Gro_Stock_ListVM item in stockListVM)
            {
                var stock = _mapper.Map<Gro_Stock_List>(item);

                if (stock.Id == 0)
                {
                    try
                    {
                        await _GroStocklistRepository.AddAsync(stock);
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {
                    // Update logic if required
                }

                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.Gro_Stock_ListId = stock.Id;
            }

            return stockListVM;
        }

        public async Task<Gro_Stock_ListVM> PostGroStockList(Gro_Stock_ListVM stockVM)
        {
            var stock = _mapper.Map<Gro_Stock_List>(stockVM);

            if (stock.Id == 0)
            {
                try
                {
                    await _GroStocklistRepository.AddAsync(stock);
                     
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                stock = await _GroStocklistRepository.SingleOrDefaultAsync(x => x.Id == stock.Id);
                stock.Qnty_on_Hand = stockVM.Qnty_on_Hand;
                stock = await _GroStocklistRepository.UpdateAsync(stock.Id, stock);
                // Update logic if required
            }

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            stockVM.Gro_Stock_ListId = stock.Id;

            return stockVM;
        }
        public async Task<Gro_Stock_ListVM> Updategrostocklastslno(Gro_Stock_ListVM controlVM)
        {
            var control = _mapper.Map<Gro_Stock_List>(controlVM);

            if (control.Id > 0)
            {

                control = await _GroStocklistRepository.SingleOrDefaultAsync(x => x.Id == control.Id);
                control.Last_Sl_No = controlVM.Last_Sl_No;
                
                control = await _GroStocklistRepository.UpdateAsync(control.Id, control);
            }

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            controlVM.Gro_Stock_ListId = control.Id;

            return controlVM;
        }
        public async Task<Gro_Stock_ListVM> UpdateGroStockQty(Gro_Stock_ListVM GroDataVM)
        {

            var gro = _mapper.Map<Gro_Stock_List>(GroDataVM);

            if (gro.Id > 0)
            {

                try
                {
                    gro = await _GroStocklistRepository.SingleOrDefaultAsync(x => x.Gro_Part_List_ID == gro.Gro_Part_List_ID);
                    gro.Qnty_on_Hand = GroDataVM.Qnty_on_Hand;


                    gro = await _GroStocklistRepository.UpdateAsync(gro.Id, gro);
                    //await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Gro_Stock_ListId = gro.Id;


            return GroDataVM;
        }
        public async Task<bool> DeleteGroStockList(long id)
        {
            var stock = await _GroStocklistRepository.SingleOrDefaultAsync(x => x.Id == id);

            if (stock != null)
            {
                try
                {
                    _GroStocklistRepository.Remove(stock);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                }
            }

            return false;
        }
        public async Task<Gro_Stock_ListVM> GetStockByGroPartListId(long groPartListId, long tenantId)
        {
            var stock = await _GroStocklistRepository.SingleOrDefaultAsync(x => x.Gro_Part_List_ID == groPartListId && x.TenantId == tenantId);

            return _mapper.Map<Gro_Stock_ListVM>(stock);
        }
        public async Task<IEnumerable<TK_DC_Inv_ContrlVM>> AllTKDCInvContrl(long tenantId)
        {
            var allData = _tK_DC_Inv_ContrlRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<TK_DC_Inv_ContrlVM>>(allData);
        }

        public async Task<List<TK_DC_Inv_ContrlVM>> MultipleTKDCInvContrl(List<TK_DC_Inv_ContrlVM> controlVM)
        {
            foreach (TK_DC_Inv_ContrlVM item in controlVM)
            {
                var control = _mapper.Map<TK_DC_Inv_Contrl>(item);

                if (control.Id == 0)
                {
                    try
                    {
                        await _tK_DC_Inv_ContrlRepository.AddAsync(control);
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }

                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.TK_DC_Inv_ContrlId = control.Id;
            }

            return controlVM;
        }

        public async Task<TK_DC_Inv_ContrlVM> PostTKDCInvContrl(TK_DC_Inv_ContrlVM controlVM)
        {
            var control = _mapper.Map<TK_DC_Inv_Contrl>(controlVM);

            if (control.Id == 0)
            {
                try
                {
                    await _tK_DC_Inv_ContrlRepository.AddAsync(control);
                    //await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                control = await _tK_DC_Inv_ContrlRepository.SingleOrDefaultAsync(x => x.Id == control.Id);
                control.TK_DC_Last_No = controlVM.TK_DC_Last_No;
                control.TK_Inv_Last_No = controlVM.TK_Inv_Last_No;
                control.DC_Enable = controlVM.DC_Enable;
                control.Inv_Print_Enable = controlVM.Inv_Print_Enable;
                control.Inv_Push_Enable = controlVM.Inv_Push_Enable;
               


                control = await _tK_DC_Inv_ContrlRepository.UpdateAsync(control.Id, control);
            }

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            controlVM.TK_DC_Inv_ContrlId = control.Id;

            return controlVM;
        }
        public async Task<TK_DC_Inv_ContrlVM> UpdateTkDclastDcandInvNo(TK_DC_Inv_ContrlVM controlVM)
        {
            var control = _mapper.Map<TK_DC_Inv_Contrl>(controlVM);

            if (control.Id > 0)
            {
                
                control = await _tK_DC_Inv_ContrlRepository.SingleOrDefaultAsync(x => x.Id == control.Id);
                control.TK_DC_Last_No = controlVM.TK_DC_Last_No;
                control.TK_Inv_Last_No = controlVM.TK_Inv_Last_No;
                



                control = await _tK_DC_Inv_ContrlRepository.UpdateAsync(control.Id, control);
            }

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            controlVM.TK_DC_Inv_ContrlId = control.Id;

            return controlVM;
        }
        public async Task<bool> DeleteTKDCInvContrl(long id)
        {
            var control = await _tK_DC_Inv_ContrlRepository.SingleOrDefaultAsync(m => m.Id == id);

            if (control != null)
            {
                try
                {
                    _tK_DC_Inv_ContrlRepository.Remove(control);
                    await _unitOfWork.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {

                }
            }

            return false;
        }
        public async Task<IEnumerable<Gro_Stock_DetVM>> AllGroStockDet(long tenantId)
        {
            var allwo = _gro_Stock_DetRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Gro_Stock_DetVM>>(allwo);
        }

        public async Task<List<Gro_Stock_DetVM>> MultipleGroStockDet(List<Gro_Stock_DetVM> GroStockDetVM)
        {
            foreach (Gro_Stock_DetVM item in GroStockDetVM)
            {
                var stockDet = _mapper.Map<Gro_Stock_Det>(item);

                if (stockDet.Id == 0)
                {
                    try
                    {
                        await _gro_Stock_DetRepository.AddAsync(stockDet);
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }

                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.Gro_Stock_DetId = stockDet.Id;
            }

            return GroStockDetVM;
        }
        public async Task<List<Gro_Stock_DetVM>> UpdateGrostockdetStatusto1stScan(List<Gro_Stock_DetVM> GroStockDetVM)
        {
            foreach (Gro_Stock_DetVM item in GroStockDetVM)
            {
                var stockDet = _mapper.Map<Gro_Stock_Det>(item);

                if (stockDet.Id > 0)
                {
                    stockDet = await _gro_Stock_DetRepository.SingleOrDefaultAsync(x => x.Part_Sl_No == item.Part_Sl_No);
                    stockDet.Sl_No_Status_ID = item.Sl_No_Status_ID;





                    stockDet = await _gro_Stock_DetRepository.UpdateAsync(stockDet.Id, stockDet);

                }

                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.Gro_Stock_DetId = stockDet.Id;
            }

            return GroStockDetVM;
        }
        public async Task<Gro_Stock_DetVM> PostGroStockDet(Gro_Stock_DetVM GroStockDetVM)
        {
            var stockDet = _mapper.Map<Gro_Stock_Det>(GroStockDetVM);

            if (stockDet.Id == 0)
            {
                try
                {
                    await _gro_Stock_DetRepository.AddAsync(stockDet);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {

            }

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroStockDetVM.Gro_Stock_DetId = stockDet.Id;

            return GroStockDetVM;
        }

        public async Task<bool> DeleteGroStockDet(long Id)
        {
            var co = await _gro_Stock_DetRepository.SingleOrDefaultAsync(m => m.Id == Id);

            if (co != null)
            {
                try
                {
                    _gro_Stock_DetRepository.Remove(co);
                    await _unitOfWork.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {

                }
            }

            return false;
        }
        public async Task<Sl_No_Status_ListVM> GetSLstatustype(long Id)
        {
            var allpp = await _slNo_Status_ListRepository.SingleOrDefaultAsync(d => d.Id == Id);
            if (allpp != null)
            {
                return _mapper.Map<Sl_No_Status_ListVM>(allpp);
            }
            return new Sl_No_Status_ListVM { Sl_No_Status_ID = -1 };
        }

        public async Task<IEnumerable<Indent_Part_Sl_NoVM>> AllIndentPartSlNo(long tenantId)
        {
            var allwo = _indent_Part_Sl_NoRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Indent_Part_Sl_NoVM>>(allwo);
        }

        public async Task<List<Indent_Part_Sl_NoVM>> MultipleIndentPartSlNo(List<Indent_Part_Sl_NoVM> indentPartSlNoVM)
        {
            foreach (Indent_Part_Sl_NoVM item in indentPartSlNoVM)
            {
                var indentPartSlNo = _mapper.Map<Indent_Part_Sl_No>(item);

                if (indentPartSlNo.Id == 0)
                {
                    try
                    {
                        await _indent_Part_Sl_NoRepository.AddAsync(indentPartSlNo);
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }

                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.Indent_Part_Sl_No_ID = indentPartSlNo.Id;
            }

            return indentPartSlNoVM;
        }

        public async Task<Indent_Part_Sl_NoVM> PostIndentPartSlNo(Indent_Part_Sl_NoVM indentPartSlNoVM)
        {
            var indentPartSlNo = _mapper.Map<Indent_Part_Sl_No>(indentPartSlNoVM);

            if (indentPartSlNo.Id == 0)
            {
                try
                {
                    await _indent_Part_Sl_NoRepository.AddAsync(indentPartSlNo);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {

            }

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            indentPartSlNoVM.Indent_Part_Sl_No_ID = indentPartSlNo.Id;

            return indentPartSlNoVM;
        }

        public async Task<bool> DeleteIndentPartSlNo(long Id)
        {
            var co = await _indent_Part_Sl_NoRepository.SingleOrDefaultAsync(m => m.Id == Id);

            if (co != null)
            {
                try
                {
                    _indent_Part_Sl_NoRepository.Remove(co);
                    await _unitOfWork.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {

                }
            }

            return false;
        }
        public async Task<IEnumerable<Gro_Indent_DispHeadVM>> AllGroIndentDispHeader(long tenantId)
        {
            var allwo = _Gro_Indent_DispHeadRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<Gro_Indent_DispHeadVM>>(allwo);
        }
        public async Task<List<Gro_Indent_DispHeadVM>> MultipleGroIndentDispheader(List<Gro_Indent_DispHeadVM> GroDataVM)
        {
            foreach (Gro_Indent_DispHeadVM item in GroDataVM)
            {
                var gro = _mapper.Map<Gro_Indent_DispHead>(item);

                if (gro.Id == 0)
                {

                    try
                    {
                        await _Gro_Indent_DispHeadRepository.AddAsync(gro);
                        await _unitOfWork.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        Exception exa = ex.InnerException;
                        string msg = ex.Message;
                    }
                }
                else
                {

                }
                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }

                item.Gro_Indent_DispHead_ID = gro.Id;

            }
            return GroDataVM;
        }
        public async Task<Gro_Indent_DispHeadVM> PostGroIndentDispHeader(Gro_Indent_DispHeadVM GroDataVM)
        {

            var gro = _mapper.Map<Gro_Indent_DispHead>(GroDataVM);

            if (gro.Id == 0)
            {

                try
                {
                    await _Gro_Indent_DispHeadRepository.AddAsync(gro);
                    await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {

            }
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                Exception exa = ex.InnerException;
                string msg = ex.Message;
            }

            GroDataVM.Gro_Indent_DispHead_ID = gro.Id;


            return GroDataVM;
        }
        


        public async Task<bool> DeleteGroIndentDispHeader(long Id)
        {
            var co = await _Gro_Indent_DispHeadRepository.SingleOrDefaultAsync(m => m.Id == Id);
            if (co != null)
            {
                try
                {
                    _Gro_Indent_DispHeadRepository.Remove(co);
                    await _unitOfWork.CommitAsync();

                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
    }
}
