using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VikoServiceManager.Controllers
{
    [Authorize]
    public class AccessCheckerController : Controller
    {
        [AllowAnonymous]
        //for all users, even not loged in
        public IActionResult AllAccess()
        {
            return View();
        }

        [Authorize]
        // loged in users
        public IActionResult AuthorizedAccess()
        {
            return View();
        }

        [Authorize(Roles ="User")]
        // user role
        public IActionResult UserAccess()
        {
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        // user role
        public IActionResult UserOrAdminAccess()
        {
            return View();
        }

        [Authorize(Policy = "UserAndAdmin")]
        // user role
        public IActionResult UserAndAdminAccess()
        {
            return View();
        }

        // Policy is described in Program.cs
        [Authorize(Policy = "Admin")]
        // admin role
        public IActionResult AdminAccess()
        {
            return View();
        }

        [Authorize(Policy = "AdminCreateAccess")]
        // admin role and claim of create
        public IActionResult AdminCreateAccess()
        {
            return View();
        }

        [Authorize(Policy = "AdminCreateEditDeleteAccess")]
        // admin role and claim of create, edit, delete (AND)
        public IActionResult AdminCreateEditDeleteAccess()
        {
            return View();
        }

        [Authorize(Policy = "AdminCreateEditDeleteAccessOrSuperAdmin")]
        // admin role and claim of create, edit, delete (AND), OR user is Super Admin
        public IActionResult AdminCreateEditDeleteAccessOrSuperAdmin()
        {
            return View();
        }

        [Authorize(Policy = "AdminWithMoreThanThousandDays")]
        // special case where user account needs to be older than 1000 days to see the page
        public IActionResult OnlyGiedrius()
        {
            return View();
        }
    }
}
