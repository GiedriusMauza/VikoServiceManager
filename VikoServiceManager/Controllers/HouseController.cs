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
                if (house == null)
                {
                    house.ResidentGroupName = "None";
                }
                else
                {
                    house.ResidentGroupName = residentGroups.FirstOrDefault(u => u.Id == house.ResidentGroup.Id).Name;
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
        public IActionResult Details()
        {

            return View();
        }

        // GET: HouseController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HouseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
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

        // GET: HouseController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HouseController/Edit/5
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

        // GET: HouseController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HouseController/Delete/5
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
