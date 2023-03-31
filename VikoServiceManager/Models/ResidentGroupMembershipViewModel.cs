using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VikoServiceManager.Models
{
    public class ResidentGroupMembershipViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public ResidentGroupViewModel ResidentGroup { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
