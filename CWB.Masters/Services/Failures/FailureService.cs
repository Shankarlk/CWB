using AutoMapper;
using CWB.Logging;
using CWB.Masters.Domain;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Failures;
using CWB.Masters.ViewModels.FailureError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.Services.Failures
{
    public class FailureService : IFailureServices
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFailureRepository _FailureRepository;
        public FailureService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork, IFailureRepository FailureRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _FailureRepository =FailureRepository;
        }
        public async Task<IEnumerable<FailureVM>> GetFailure(long tenantId)
        {
            var allDocuType = _FailureRepository.GetRangeAsync(d => d.TenantId == tenantId);
            return _mapper.Map<IEnumerable<FailureVM>>(allDocuType);
        }

        public async Task<FailureVM> PostFailure(FailureVM Failures)
        {
            var doctype = _mapper.Map<Failure>(Failures);
            if (doctype.Id == 0)
            {
                try
                {
                    await _FailureRepository.AddAsync(doctype);
                }
                catch (Exception ex)
                {
                    Exception exa = ex.InnerException;
                    string msg = ex.Message;
                }
            }
            else
            {
                var doctypeid = await _FailureRepository.SingleOrDefaultAsync(x => x.Id == doctype.Id);
                if (doctypeid == null)
                {
                    return Failures;
                }
                doctype = await _FailureRepository.UpdateAsync(doctype.Id, doctype);
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
            Failures.FailureId = doctype.Id;
            return Failures;
        }
    }
}
