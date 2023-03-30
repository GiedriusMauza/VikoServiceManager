namespace VikoServiceManager.Models
{
    public class HouseServiceViewModel
    {
        public int Id { get; set; }
        public HouseViewModel House { get; set; }
        public ServiceViewModel Service { get; set; }
    }
}
