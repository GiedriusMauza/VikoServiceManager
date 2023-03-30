using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VikoServiceManager.Models
{
    public class HouseViewModel
    {
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        [DefaultValue(true)]
        public ResidentGroup? ResidentGroup { get; set; }
        [NotMapped]
        public string? ResidentGroupName { get; set; }
        [NotMapped]
        public string? ServiceName { get; set; }

        [NotMapped]
        public string? ResidentGroupSelected { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ResidentGroupList { get; set; }

        [NotMapped]
        public string? ServiceSelected { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? ServiceSelectedList { get; set; }
    }
}
