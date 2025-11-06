using System.ComponentModel.DataAnnotations;

namespace CWB.Identity.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "New Password don't match Confirm Password.")]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}
