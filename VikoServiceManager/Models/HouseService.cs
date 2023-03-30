namespace VikoServiceManager.Models
{
    public class HouseService
    {
        public int Id { get; set; }
        public HouseViewModel House { get; set; }
        public Service Service { get; set; }
    }
}
