﻿using CWB.CompanySettings.ViewModels.Designations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.Designations
{
    public interface IDesignationService
    {
        IEnumerable<DesignationVM> GetDesignations(long TenantId);

        Task<DesignationVM> Designation(DesignationVM designationVM);

        bool CheckDesignationExisit(CheckDesignationVM checkDesignationVM);
    }
}
