using CodeFirstASPCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CodeFirstASPCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly StudentDBContext studentDB;

        public HomeController(StudentDBContext studentDB)
        {
            this.studentDB = studentDB;
        }
        public async Task<IActionResult> Index()
        {
            var stdData = await studentDB.Students.ToListAsync();
            return View(stdData);
        }

        public IActionResult Create()
        {
            List<SelectListItem> Gender = new List<SelectListItem>()
            {
                new SelectListItem {Value ="Male", Text="Male"},
                new SelectListItem {Value ="Female", Text="Female"}
            };

            ViewBag.Gender = Gender;
            return View();
        }

        //[HttpPost]
        //public IActionResult Create(Student student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        studentDB.Students.Add(student);
        //        studentDB.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(student);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                await studentDB.Students.AddAsync(student);
                await studentDB.SaveChangesAsync();
                TempData["insert_success"] = "Inserted..";
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public async Task<IActionResult> Details(int? id)
        {
            
            if(id == null || studentDB.Students == null)
            {
                return NotFound();
            }

            var stdData = await studentDB.Students.FirstOrDefaultAsync(x => x.Id == id);

            if (stdData == null)
            {
                return NotFound();
            }

            return View(stdData);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            List<SelectListItem> Gender = new List<SelectListItem>()
            {
                new SelectListItem {Value ="Male", Text="Male"},
                new SelectListItem {Value ="Female", Text="Female"}
            };

            ViewBag.Gender = Gender;


            if (id == null || studentDB.Students == null)
            {
                return NotFound();
            }

            var stdData = await studentDB.Students.FindAsync(id);

            if (stdData == null)
            {
                return NotFound();
            }

            return View(stdData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Student student)
        {
            if (id != student.Id)
            {
                NotFound();
            }

            if (ModelState.IsValid)
            {
                studentDB.Update(student);
                await studentDB.SaveChangesAsync();
                TempData["update_success"] = "Updated..";
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || studentDB.Students == null)
            {
                return NotFound();
            }

            var stdData = await studentDB.Students.FirstOrDefaultAsync(x=> x.Id == id);

            if (stdData == null)
            {
                return NotFound();
            }

            return View(stdData);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {

            if (id == null || studentDB.Students == null)
            {
                return NotFound();
            }

            var stdData = await studentDB.Students.FindAsync(id);

            if (stdData != null)
            {
                studentDB.Students.Remove(stdData);
            }
            await studentDB.SaveChangesAsync();
            TempData["delete_success"] = "Deleted..";

            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
