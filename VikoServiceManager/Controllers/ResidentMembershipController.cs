using IdentityManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VikoServiceManager.Models;

namespace VikoServiceManager.Controllers
{
    [Authorize]
    public class ResidentMembershipController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ResidentMembershipController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "Admin,Manager")]
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

                var membership = residentMemberships.FirstOrDefault(u => u.ApplicationUser.Id.Equals(user.Id));
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

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult AssignResidentGroup(string userId)
        {
            var resGroupFromDb = _db.ResidentGroup.ToList();

            List<SelectListItem> groupItems = new List<SelectListItem>();
            foreach (var item in resGroupFromDb)
            {
                groupItems.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            // update               
            var objFromDb = _db.ApplicationUser.FirstOrDefault(x => x.Id == userId);
            objFromDb.ResidentGroupList = groupItems;
            return View(objFromDb);


        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public IActionResult AssignResidentGroup(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(applicationUser.Id))
                {  // update
                    var objUserFromDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == applicationUser.Id);
                    var objGroupFromDb = _db.ResidentGroup.FirstOrDefault(u => u.Id.ToString() == applicationUser.ResidentGroupSelected);
                    var objMembershipFromDb = _db.ResidentGroupMembership.FirstOrDefault(u => u.ApplicationUser.Id == objUserFromDb.Id);
                    if (objUserFromDb == null)
                    {
                        TempData[SD.Error] = "User not found!";
                        return RedirectToAction(nameof(Index));
                    }

                    // delete entry if it already exists
                    if (objMembershipFromDb != null)
                    {
                        _db.ResidentGroupMembership.Remove(objMembershipFromDb);
                        _db.SaveChanges();
                    }
                    ResidentGroupMembershipViewModel residentGroupMembershipViewModel = new ResidentGroupMembershipViewModel()
                    {
                        ResidentGroup = objGroupFromDb,
                        ApplicationUser = objUserFromDb
                    };

                    _db.ResidentGroupMembership.Update(residentGroupMembershipViewModel);
                    _db.SaveChanges();

                    TempData[SD.Success] = "Resident Group updated successfully!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

    }
}
