using IdentityManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using VikoServiceManager.Models;

namespace VikoServiceManager.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ServiceController(ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var services = _db.Service.ToList();
            var applicationUser = _db.ApplicationUser.ToList();
            foreach (var service in services)
            {
                if (applicationUser != null && service.ApplicationUser != null)
                {
                    service.ManagerName = applicationUser.FirstOrDefault(u => u.Id == service.ApplicationUser.Id).Name;
                }
                else
                {
                    service.ManagerName = "None";
                }
            }


            return View(services);
        }

        [HttpGet]
        public IActionResult EditPrice(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                // update
                var objFromDb = _db.Service.FirstOrDefault(x => x.Id.Equals(id));
                return View(objFromDb);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPrice(ServiceViewModel serviceObj)
        {

            // update
            var objServiceFromDb = _db.Service.FirstOrDefault(u => u.Id == serviceObj.Id);
            if (objServiceFromDb == null)
            {
                TempData[SD.Error] = "Service not found!";
                return RedirectToAction(nameof(Index));
            }

            objServiceFromDb.Price = serviceObj.Price;
            _db.SaveChanges();

            TempData[SD.Success] = "Price updated successfully!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Upsert(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                // update
                var objFromDb = _db.Service.FirstOrDefault(x => x.Id.Equals(id));
                return View(objFromDb);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ServiceViewModel serviceObj)
        {

            if (_db.Service.Any(u => u.ServiceName == serviceObj.ServiceName))
            {
                //error
                TempData[SD.Error] = "Service already exists!";
                return RedirectToAction(nameof(Index));
            }
            if (string.IsNullOrEmpty(Convert.ToString(serviceObj.Id)) || serviceObj.Id == 0)
            {
                //create
                _db.Service.Add(new ServiceViewModel() { ServiceName = serviceObj.ServiceName, Price = serviceObj.Price });
                _db.SaveChanges();
                TempData[SD.Success] = "Service created successfully!";
            }
/*            else
            {
                // update
                var objRoleFromDb = _db.Roles.FirstOrDefault(u => u.Id == serviceObj.Id);
                if (objRoleFromDb == null)
                {
                    TempData[SD.Error] = "Role not found!";
                    return RedirectToAction(nameof(Index));
                }
                objRoleFromDb.Name = serviceObj.ServiceName;
                objRoleFromDb.NormalizedName = serviceObj.ServiceName.ToUpper();
                var result = await _roleManager.UpdateAsync(objRoleFromDb);
                TempData[SD.Success] = "Role updated successfully!";
            }*/
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int serviceId)
        {
            var objFromDb = _db.Service.FirstOrDefault(u => u.Id == serviceId);
            if (objFromDb == null)
            {
                TempData[SD.Error] = "Service not found!";
                return RedirectToAction(nameof(Index));
            }

            var userGroupsForThisHouse = _db.HouseService.Where(u => u.Service.Id == serviceId).Count();
            if (userGroupsForThisHouse > 0)
            {
                TempData[SD.Error] = "Cannot delete this service, since there are house assigned for this service!";
                return RedirectToAction(nameof(Index));
            }
            _db.Service.Remove(objFromDb);
            _db.SaveChanges();
            TempData[SD.Success] = "Service deleted successfully!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignManager(string id)
        {
            // Create list for select element
            var appUserFromDb = _db.ApplicationUser.ToList();
            

            List<SelectListItem> usersItems = new List<SelectListItem>();
            foreach (var item in appUserFromDb)
            {
                var roleName = _db.UserRoles.FirstOrDefault(u => u.UserId == item.Id);

                var rolesValues = _db.Roles.FirstOrDefault(u => u.Id == roleName.RoleId).Name;
                if (rolesValues == "Manager")
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult AssignManager(ServiceViewModel serviceObj)
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
        }

    }
}
