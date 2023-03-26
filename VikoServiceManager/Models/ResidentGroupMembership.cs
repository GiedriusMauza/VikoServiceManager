namespace VikoServiceManager.Models
{
    public class ResidentGroupMembership
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ResidentGroup ResidentGroup { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
