namespace VikoServiceManager.Models
{
    public class Price
    {
        public int Id { get; set; }
        public float PriceAmount{ get; set; }
        public Service Service { get; set; }
    }
}
