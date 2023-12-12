using AutoMapper;
using CWB.CommonUtils.Common;
using CWB.Logging;
using CWB.Masters.Infrastructure;
using CWB.Masters.MastersUtils;
using CWB.Masters.Repositories.Company;
using CWB.Masters.ViewModels.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.Services.Company
{
    public class CompanyService : ICompanyService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDivisionRepository _divisionRepository;
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork,
            IDivisionRepository divisionRepository, ICompanyRepository companyRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _divisionRepository = divisionRepository;
            _companyRepository = companyRepository;
        }

        public bool CheckIfCompanyExisit(CheckCompanyVM checkCompanyVM)
        {
            var company = _companyRepository.GetRangeAsync(c => c.Name == checkCompanyVM.CompanyName &&
            c.TenantId == checkCompanyVM.TenantId);
            if (!company.Any())
            {
                return false;
            }
            return (company.First().Id != checkCompanyVM.CompanyId);
        }

        public bool CheckIfDivisionExisit(CheckDivisionVM checkDivisionVM)
        {
            var department = _divisionRepository.GetRangeAsync(d => d.Name == checkDivisionVM.DivisionName &&
            d.TenantId == checkDivisionVM.TenantId && d.CompanyId == checkDivisionVM.CompanyId);
            if (!department.Any())
            {
                return false;
            }
            return (department.First().Id != checkDivisionVM.DivisionId);
        }

        public async Task<CompanyVM> Company(CompanyVM companyVM)
        {
            var company = _mapper.Map<Domain.Company>(companyVM);
            if (company.Id == 0)
            {
                await _companyRepository.AddAsync(company);
            }
            else
            {
                company = await _companyRepository.UpdateAsync(company.Id, company);
            }
            await _unitOfWork.CommitAsync();
            var division = _mapper.Map<Domain.Division>(companyVM);
            division.CompanyId = company.Id;
            if (division.Id == 0)
            {
                await _divisionRepository.AddAsync(division);
            }
            else
            {
                division = await _divisionRepository.UpdateAsync(division.Id, division);
            }
            await _unitOfWork.CommitAsync();
            companyVM.CompanyId = company.Id;
            companyVM.DivisionId = division.Id;
            return companyVM;
        }

        public async Task<IEnumerable<CompaniesVM>> GetCompaniesByCompanyNTenant(long companyID, long tenantID)
        {
            var companies = await _divisionRepository.GetAllDivisionByCompanyNTenantAsync(companyID, tenantID);
            return _mapper.Map<IEnumerable<CompaniesVM>>(companies);
        }

        public async Task<IEnumerable<CompaniesVM>> GetCompaniesByTenant(long tenantID)
        {
            var companies = await _divisionRepository.GetAllDivisionByTenantAsync(tenantID);
            return _mapper.Map<IEnumerable<CompaniesVM>>(companies);
        }

        public IEnumerable<CompanyTypeVM> GetCompanyTypes()
        {
            var companyTypes = Enum.GetValues(typeof(CompanyType))
                         .Cast<CompanyType>()
                         .Select(t => new CompanyTypeVM { CompanyType = t.GetEnumDescription(), CompanyTypeValue = t.ToString() });
            return companyTypes;
        }
    }
}
