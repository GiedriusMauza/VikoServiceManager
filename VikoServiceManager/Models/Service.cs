using Microsoft.Build.Framework;

namespace VikoServiceManager.Models
{
    public class Service
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? ServiceName { get; set; }
        [Required]
        public float Price { get; set; }
    }
}
