using IdentityManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Upsert(ResidentGroup residentGroup)
        {

            if (_db.Service.Any(u => u.ServiceName == residentGroup.Name))
            {
                //error
                TempData[SD.Error] = "Resident group already exists!";
                return RedirectToAction(nameof(Index));
            }
            if (string.IsNullOrEmpty(Convert.ToString(residentGroup.Id)) || residentGroup.Id == 0)
            {
                //create
                _db.ResidentGroup.Add(new ResidentGroup() { Name = residentGroup.Name, Description = residentGroup.Description });
                _db.SaveChanges();
                TempData[SD.Success] = "Resident group created successfully!";
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
        public IActionResult Delete(int userId)
        {
            var objFromDb = _db.ResidentGroup.FirstOrDefault(u => u.Id == userId);
            if (objFromDb == null)
            {
                // error Resident Grouop not found
                TempData[SD.Error] = "Resident Group not found!";
                return RedirectToAction(nameof(Index));
            }

            var userGroupsForThisHouse = _db.ResidentGroupMembership.Where(u => u.ResidentGroup.Id == userId).Count();
            if (userGroupsForThisHouse > 0)
            {
                TempData[SD.Error] = "Cannot delete this service, since there are house assigned for this service!";
                return RedirectToAction(nameof(Index));
            }
            _db.ResidentGroup.Remove(objFromDb);
            _db.SaveChanges();
            TempData[SD.Success] = "Resident Group deleted successfully!";

            return RedirectToAction(nameof(Index));
        }
    }
}
