using System.ComponentModel.DataAnnotations;

namespace VikoServiceManager.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
