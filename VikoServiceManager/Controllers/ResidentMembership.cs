using IdentityManager.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VikoServiceManager.Controllers
{
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


        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Edit(int id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ResidentMembership/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ResidentMembership/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
