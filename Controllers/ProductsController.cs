using Microsoft.AspNetCore.Mvc;
using mini_store.Data;
using mini_store.Models;

namespace mini_store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();

            var categories = _context.categories.ToList();
            ViewBag.categories = categories;

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            var categories = _context.categories.ToList();
            ViewBag.categories = categories;

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
