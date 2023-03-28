using IdentityManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VikoServiceManager.Models;

namespace VikoServiceManager.Controllers
{
    public class ServiceController : Controller
    {

        private readonly ApplicationDbContext _db;

        public ServiceController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var services = _db.Service.ToList();
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
        public async Task<IActionResult> EditPrice(Service serviceObj)
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
        public IActionResult Upsert(Service serviceObj)
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
                _db.Service.Add(new Service() { ServiceName = serviceObj.ServiceName, Price = serviceObj.Price });
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


    }
}
