using IdentityManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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


    }
}
