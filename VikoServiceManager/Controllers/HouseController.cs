using Azure.Core;
using IdentityManager.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
                    house.ServiceName = service.Service.ServiceName;
                }
            }
            return View(houseList);
        }

        [HttpGet]
        public IActionResult Upsert(int houseId)
        {
            // Create list for select element
            var resGroupFromDb = _db.ResidentGroup.ToList();
            var serviceFromDb = _db.Service.ToList();

            List<SelectListItem> groupsItems = new List<SelectListItem>();
            foreach (var item in resGroupFromDb)
            {
                groupsItems.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            List<SelectListItem> servcieItems = new List<SelectListItem>();
            foreach (var item in serviceFromDb)
            {
                servcieItems.Add(new SelectListItem()
                {
                    Text = item.ServiceName,
                    Value = item.Id.ToString()
                });
            }

            HouseViewModel houseViewModel = new HouseViewModel()
            {
                ResidentGroupList = groupsItems,
                ServiceSelectedList = servcieItems

            };

            if (String.IsNullOrEmpty(Convert.ToString(houseId)) || houseId == 0)
            {
                return View(houseViewModel);
            }
            else
            {
                // update               
                var objFromDb = _db.House.FirstOrDefault(x => x.Id == houseId);
                objFromDb.ResidentGroupList = houseViewModel.ResidentGroupList;
                objFromDb.ServiceSelectedList = houseViewModel.ServiceSelectedList;
                return View(objFromDb);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(HouseViewModel houseObj)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Convert.ToString(houseObj.Id)) || houseObj.Id == 0)
                {
                    //create
                    var objGroupFromDb = _db.ResidentGroup.FirstOrDefault(u => u.Id.ToString() == houseObj.ResidentGroupSelected);
                    _db.House.Add(new HouseViewModel()
                    {
                        Address = houseObj.Address,
                        City = houseObj.City,
                        Region = houseObj.Region,
                        PostalCode = houseObj.PostalCode,
                        ResidentGroup = objGroupFromDb,

                    });
                    _db.SaveChanges();
                    TempData[SD.Success] = "House Entry created successfully!";
                }
                else
                {


                    // update
                    var objHouseFromDb = _db.House.FirstOrDefault(u => u.Id == houseObj.Id);
                    var objGroupFromDb = _db.ResidentGroup.FirstOrDefault(u => u.Id.ToString() == houseObj.ResidentGroupSelected);
                    var objServiceFromDb = _db.Service.FirstOrDefault(u => u.Id.ToString() == houseObj.ServiceSelected);
                    if (objHouseFromDb == null)
                    {
                        TempData[SD.Error] = "Role not found!";
                        return RedirectToAction(nameof(Index));
                    }
                    objHouseFromDb.Address = houseObj.Address;
                    objHouseFromDb.City = houseObj.City;
                    objHouseFromDb.Region = houseObj.Region;
                    objHouseFromDb.PostalCode = houseObj.PostalCode;
                    objHouseFromDb.ResidentGroup = objGroupFromDb;


                    if (objServiceFromDb != null)
                    {
                        // delete servcie if it already exists
                        var houseService = _db.HouseService.FirstOrDefault(u => u.House.Id == objHouseFromDb.Id); // association between house and service
                        if (houseService != null)
                        {
                            _db.HouseService.Remove(houseService);
                        }
                        _db.HouseService.Add(new HouseService()
                        {
                            House = objHouseFromDb,
                            Service = objServiceFromDb
                        });
                    }

                    _db.House.Update(objHouseFromDb);
                    _db.SaveChanges();

                    TempData[SD.Success] = "House updated successfully!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(houseObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int houseId)
        {
            var objFromDb = _db.House.FirstOrDefault(u => u.Id == houseId);
            if (objFromDb == null)
            {
                TempData[SD.Error] = "Service not found!";
                return RedirectToAction(nameof(Index));
            }
            _db.House.Remove(objFromDb);
            _db.SaveChanges();
            TempData[SD.Success] = "House deleted successfully!";

            return RedirectToAction(nameof(Index));
        }

    }
}
