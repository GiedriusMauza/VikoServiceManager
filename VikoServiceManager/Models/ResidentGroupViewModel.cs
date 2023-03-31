using System.ComponentModel.DataAnnotations.Schema;

namespace VikoServiceManager.Models
{
    public class ResidentGroupViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public float? ServicePrice { get; set; }
    }
}
