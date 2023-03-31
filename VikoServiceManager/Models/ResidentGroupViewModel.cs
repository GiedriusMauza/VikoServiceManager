using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VikoServiceManager.Models
{
    public class ResidentGroupViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [NotMapped]
        public float? ServicePrice { get; set; }
    }
}
