using IdentityManager.Data;

namespace VikoServiceManager.Authorize
{
    public class NumberOfDaysForAccount : INumberOfDaysForAccount
    {
        private readonly ApplicationDbContext _db;

        public NumberOfDaysForAccount(ApplicationDbContext db)
        {
            _db = db;
        }

        // must return number more than a 1000 to display special page
        public int Get(string userId)
        {
            var user = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);       
            if (user == null && user.DateCreated != DateTime.MinValue)
            {
                return (DateTime.Today - user.DateCreated).Days;
            }
            return 0;
        }
    }
}
