using IdentityManager.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VikoServiceManager.Models;

namespace VikoServiceManager.Controllers
{
    public class ResidentGroupController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ResidentGroupController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: ResidentGroupController
        public IActionResult Index()
        {
            var groups = _db.ResidentGroup.ToList();
            return View(groups);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (String.IsNullOrEmpty(Convert.ToString(id)))
            {
                return View();
            }
            else
            {
                // update
                var objFromDb = _db.ResidentGroup.FirstOrDefault(x => x.Id == id);
                return View(objFromDb);
            }
        }

        // GET: ResidentGroupController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ResidentGroupController/Create
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

        // GET: ResidentGroupController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: ResidentGroupController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
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

        // GET: ResidentGroupController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: ResidentGroupController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
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
