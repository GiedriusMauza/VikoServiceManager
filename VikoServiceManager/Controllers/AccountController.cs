using IdentityManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VikoServiceManager.Models;

namespace VikoServiceManager.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _db;


        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnurl = null)
        {
            // Create list for select element
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = "Manager",
                Text = "Manager"
            });
            listItems.Add(new SelectListItem()
            {
                Value = "User",
                Text = "User"
            });
            listItems.Add(new SelectListItem()
            {
                Value = "Admin",
                Text = "Admin"
            });

            ViewData["ReturnUrl"] = returnurl;
            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                RoleList = listItems
            };
            return View(registerViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            // validates if all fields are filled
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name };
                var result = await _userManager.CreateAsync(user, model.Password); // automatically encripts the password

                if (result.Succeeded)
                {
                    // Role 
                    if (model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == "Admin")
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");

                    }
                    else if (model.RoleSelected == "Manager")
                    {
                        await _userManager.AddToRoleAsync(user, "Manager");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    /*                    await _signInManager.SignInAsync(user, isPersistent: false);*/
                    /*User registerViewModel = new RegisterViewModel()*/
                    return LocalRedirect(returnurl);
                }
                AddErrors(result);
            }

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = "Admin",
                Text = "Admin"
            });
            listItems.Add(new SelectListItem()
            {
                Value = "User",
                Text = "User"
            });

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnurl);
                }
                else if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attemt");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult AssignResidentGroup(string id)
        {
            // Create list for select element
            var appUserFromDb = _db.ApplicationUser.ToList();


            List<SelectListItem> usersItems = new List<SelectListItem>();
            foreach (var item in appUserFromDb)
            {
                var roleName = _db.UserRoles.FirstOrDefault(u => u.UserId == item.Id);

                var rolesValues = _db.Roles.FirstOrDefault(u => u.Id == roleName.RoleId).Name;
                if (rolesValues == "User")
                {
                    usersItems.Add(new SelectListItem()
                    {
                        Text = item.Name + ", " + item.Email,
                        Value = item.Id
                    });
                }

            }

            ServiceViewModel serviceViewModel = new ServiceViewModel()
            {
                ManagerList = usersItems

            };

            if (String.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                // update
                var objFromDb = _db.Service.FirstOrDefault(x => x.Id.ToString() == id);
                objFromDb.ManagerList = serviceViewModel.ManagerList;
                return View(objFromDb);
            }
        }

        /*[HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult AssignResidentGroup(ServiceViewModel serviceObj)
        {
            // update
            var objServiceFromDb = _db.Service.FirstOrDefault(u => u.Id == serviceObj.Id);
            var objManagerFromDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == serviceObj.ManagerSelected);

            if (objServiceFromDb == null)
            {
                TempData[SD.Error] = "Service not found!";
                return RedirectToAction(nameof(Index));
            }

            objServiceFromDb.ApplicationUser = objManagerFromDb;
            _db.Service.Update(objServiceFromDb);
            _db.SaveChanges();

            TempData[SD.Success] = "Manager updated successfully!";

            return RedirectToAction(nameof(Index));
        }*/



        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
