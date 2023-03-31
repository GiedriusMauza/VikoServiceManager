using Microsoft.Build.Framework;

namespace VikoServiceManager.Models
{
    public class HouseServiceViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public HouseViewModel? House { get; set; }
        [Required]
        public ServiceViewModel? Service { get; set; }
    }
}
