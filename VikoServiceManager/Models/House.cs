namespace VikoServiceManager.Models
{
    public class House
    {
        public int Id { get; set; }
        public ResidentGroup ResidentGroup { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }

    }
}
