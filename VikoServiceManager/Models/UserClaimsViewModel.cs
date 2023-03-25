namespace VikoServiceManager.Models
{
    public class UserClaimsViewModel
    {
        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }

        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
    }

    public class UserClaim
    {
        public string ClaimType { get; set; }
        public bool IsSelected { get; set;}
    }
}
