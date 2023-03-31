using IdentityManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VikoServiceManager.Controllers
{
    [Authorize]
    public class ResidentMembership : Controller
    {
        private readonly ApplicationDbContext _db;

        public ResidentMembership(ApplicationDbContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            var userList = _db.ApplicationUser.ToList();

            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            var residentMemberships = _db.ResidentGroupMembership.ToList();
            var residentGroups = _db.ResidentGroup.ToList();
            foreach (var user in userList)
            {
                var role = userRole.FirstOrDefault(u => u.UserId == user.Id);
                if (role == null)
                {
                    user.Role = "None";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;
                }

                var membership = residentMemberships.FirstOrDefault(u => u.ApplicationUser.Id.Equals(user.Id) );
                if (membership == null)
                {
                    user.ResidentGroupName = "None";
                }
                else
                {
                    user.ResidentGroupName = residentGroups.FirstOrDefault(u => u.Id == membership.ResidentGroup.Id).Name;
                }
            }
            return View(userList);
        }

    }
}
