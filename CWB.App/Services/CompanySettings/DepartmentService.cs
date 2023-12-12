using CWB.App.AppUtils;
using CWB.App.Models.Departments;
using CWB.CommonUtils.Common;
using CWB.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.CompanySettings
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ILoggerManager _logger;
        private readonly ApiUrls _apiUrls;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly long tenantId;

        public DepartmentService(ILoggerManager logger, IOptions<ApiUrls> apiUrlsOptions, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _apiUrls = apiUrlsOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            tenantId = long.Parse(AppUtil.GetTenantId(_httpContextAccessor.HttpContext.User));
        }
        public async Task<IEnumerable<DepartmentListVM>> GetDepartments(long Id)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/departments/{Id}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<DepartmentListVM>>.GetAsync(uri, headers);
        }
    }
}
