using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mini_store.Data;
using mini_store.Models;

namespace mini_store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // عرض المنتجات
        public IActionResult Index(string? searchTerm)
        {
            // ==========================
            // Cookies (تمرين 31 و 32)
            // ==========================

            var customersName = Request.Cookies["Costumers"];
            Console.WriteLine("CustomersName: " + customersName);

            var courseName = Request.Cookies["Course"];
            Console.WriteLine("CourseName: " + courseName);

            var orderId = Request.Cookies["Order"];
            Console.WriteLine("OrderId: " + orderId);
            // قراءة الـ Session

            var userName = HttpContext.Session.GetString("UserName");
            Console.WriteLine("UserName: " + userName);

            var userId = HttpContext.Session.GetInt32("UserId");
            Console.WriteLine("UserId: " + userId);

            // ==========================

            ViewBag.Categories = _context.Categories.ToList();

            IQueryable<Product> products = _context.Products;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                products = products.Where(p => p.Name.Contains(searchTerm));
            }

            ViewBag.CurrentSearch = searchTerm;

            return View(products.ToList());
        }

        // إنشاء منتج
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            if (product.ImageFile != null)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(product.ImageFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    product.ImageFile.CopyTo(stream);
                }

                product.Image = fileName;
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // تعديل
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

            ViewBag.Categories = _context.Categories.ToList();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            var oldProduct = _context.Products
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == product.Id);

            if (oldProduct == null)
                return NotFound();

            if (product.ImageFile != null)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(product.ImageFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    product.ImageFile.CopyTo(stream);
                }

                product.Image = fileName;

                if (!string.IsNullOrEmpty(oldProduct.Image))
                {
                    string oldImage = Path.Combine(folder, oldProduct.Image);

                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                    }
                }
            }
            else
            {
                product.Image = oldProduct.Image;
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // حذف
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

            if (!string.IsNullOrEmpty(product.Image))
            {
                string imagePath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    "images",
                    product.Image);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}