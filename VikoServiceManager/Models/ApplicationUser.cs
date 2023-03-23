using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VikoServiceManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
    }
}