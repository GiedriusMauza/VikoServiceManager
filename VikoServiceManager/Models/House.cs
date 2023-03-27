using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace VikoServiceManager.Models
{
    public class House
    {
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public ResidentGroup? ResidentGroup { get; set; }

        [NotMapped]
        public string? ResidentGroupName { get; set; }
        [NotMapped]
        public string? ServiceName { get; set; }
    }
}
