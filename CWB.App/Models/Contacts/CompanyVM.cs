using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CWB.App.Models.Contacts
{
    public class CompanyVM
    {
        public long CompanyId { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(25, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        [Remote(
            "IsCompanyExist",
            "Contacts",
            AdditionalFields = "CompanyId",
            ErrorMessage = "{0} already exists. Please enter a different {0}.",
            HttpMethod = "POST"
        )]
        public string CompanyName { get; set; }

        [Display(Name = "Customer / Supplier")]
        [Required(ErrorMessage = "Please enter {0}.")]
        public string CompanyType { get; set; }
        public long DivisionId { get; set; }
        [Display(Name = "Division")]
        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(25, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        [Remote(
            "IsDivisionExist",
            "Contacts",
            AdditionalFields = "CompanyId,DivisionId",
            ErrorMessage = "{0} already exists. Please enter a different {0}.",
            HttpMethod = "POST"
        )]
        public string DivisionName { get; set; }

        [Display(Name = "Location")]
        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(25, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Location { get; set; }

        [Display(Name = "Notes")]
        [StringLength(250, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Notes { get; set; }
        public long TenantId { get; set; }
    }
}
