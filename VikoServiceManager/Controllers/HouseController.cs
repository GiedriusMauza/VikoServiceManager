using IdentityManager.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VikoServiceManager.Models;

namespace VikoServiceManager.Controllers
{
    public class HouseController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HouseController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int id)
        {

            var houseList = _db.House.ToList();

            var houseServiceList = _db.HouseService.ToList();
            var serviceList = _db.Service.ToList();


            var residentGroups = _db.ResidentGroup.ToList();
            foreach (var house in houseList)
            {
                if (house != null && house.ResidentGroup != null)
                {
                    house.ResidentGroupName = residentGroups.FirstOrDefault(u => u.Id == house.ResidentGroup.Id).Name;
                }
                else
                {
                    house.ResidentGroupName = "None";
                }

                var service = houseServiceList.FirstOrDefault(u => u.House.Id.Equals(house.Id));
                if (service == null)
                {
                    house.ServiceName = "None";
                }
                else
                {
                    house.ServiceName = serviceList.FirstOrDefault(u => u.Id == service.Id).ServiceName;
                }
            }
            return View(houseList);
        }

        [HttpGet]
        public IActionResult Upsert(string houseId)
        {
            if (String.IsNullOrEmpty(houseId))
            {
                return View();
            }
            else
            {
                // update
                var objFromDb = _db.House.FirstOrDefault(x => x.Id.Equals(houseId));
                return View(objFromDb);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(House houseObj)
        {

            if (_db.House.Any(u => u.Id == houseObj.Id))
            {
                //error
                TempData[SD.Error] = "House already exists!";
                return RedirectToAction(nameof(Index));
            }
            if (string.IsNullOrEmpty(Convert.ToString(houseObj.Id)) || houseObj.Id == 0)
            {
                //create
                _db.House.Add(new House() {
                    Address = houseObj.Address,
                    City = houseObj.City,
                    Region = houseObj.Region,
                    PostalCode = houseObj.PostalCode
                });
                _db.SaveChanges();
                TempData[SD.Success] = "House Entry created successfully!";
            }
            else
            {
/*                // update
                var objRoleFromDb = _db.Roles.FirstOrDefault(u => u.Id == houseObj.Id);
                if (objRoleFromDb == null)
                {
                    TempData[SD.Error] = "Role not found!";
                    return RedirectToAction(nameof(Index));
                }
                objRoleFromDb.Name = houseObj.ServiceName;
                objRoleFromDb.NormalizedName = houseObj.ServiceName.ToUpper();
                var result = await _roleManager.UpdateAsync(objRoleFromDb);
                TempData[SD.Success] = "Role updated successfully!";*/
            }
            return RedirectToAction(nameof(Index));
        }

/*        [HttpPost]
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
        }*/

    }
}
