using Microsoft.AspNetCore.Mvc;
using mini_store.Data;
using mini_store.Models;

namespace mini_store.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

          // 
        public static int Amount = 100;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // عرض جميع الفئات
        public IActionResult Index()
        {
            // ==========================
            // Cookies (تمرين 31 و 32)
            // ==========================

            CookieOptions options = new CookieOptions();

            options.Expires = DateTime.Now.AddDays(7);

            Response.Cookies.Append("Costumers", "Ahmed", options);
            Response.Cookies.Append("Course", "Asp.net Core MVC");
            Response.Cookies.Append("Order", "250");

            // ==========================
            // Session (تمرين 33)
            // ==========================

            HttpContext.Session.SetString("UserName", "Ahmed");
            HttpContext.Session.SetInt32("UserId", 105);

            // ==========================

            var categories = _context.Categories.ToList();

            return View(categories);
        }

        // إضافة فئة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categories category)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // تعديل
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categories category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Update(category);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // حذف
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}