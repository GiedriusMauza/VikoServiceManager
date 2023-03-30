using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace VikoServiceManager.Models
{
    public class ServiceViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? ServiceName { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public ApplicationUser? ApplicationUser { get; set; }

        [NotMapped]
        public string? ManagerName { get; set;}

        [NotMapped]
        public string? ManagerSelected { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ManagerList { get; set; }
    }
}
