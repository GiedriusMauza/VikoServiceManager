using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace VikoServiceManager.Models
{
    public class ResidentGroupMembershipViewModel
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ResidentGroupViewModel ResidentGroup { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
