using Microsoft.AspNetCore.Mvc;

namespace mini_store.Controllers
{
    public class HomeController : Controller
    {
        private static readonly dynamic[] _categories =
        {
            new { Id = 0, Name = "ورود طبيعية", Icon = "fa-solid fa-seedling" },
            new { Id = 1, Name = "باقات ورد", Icon = "fa-solid fa-gift" },
            new { Id = 2, Name = "هدايا ورد", Icon = "fa-solid fa-heart" }
        };

        private static readonly dynamic[] _products =
        {
            new
            {
                CategoryId = 0,
                Name = "ورد توليب وردي",
                Price = 120,
                Description = "باقة ورد توليب طبيعي لمسة من الجمال تضئ يومك",
                Image = "rose1.jpg"
            },
            new
            {
                CategoryId = 0,
                Name = "ورد ليلي وردي",
                Price = 100,
                Description = "ورد أنيق للمناسبات",
                Image = "rose2.jpg"
            },
            new
            {
                CategoryId = 1,
                Name = "بوكيه ليلي فاخر",
                Price = 250,
                Description = "بوكيه فاخر",
                Image = "bouquet1.jpg"
            },
            new
            {
                CategoryId = 1,
                Name = "بوكيه ورد وردي",
                Price = 220,
                Description = "بوكيه أنيق",
                Image = "bouquet2.jpg"
            },
            new
            {
                CategoryId = 2,
                Name = "هدية ورد وشوكولاتة",
                Price = 300,
                Description = "هدية جميلة",
                Image = "gift1.jpg"
            },
            new
            {
                CategoryId = 2,
                Name = "ورد مع شوكولاتة فاخر",
                Price = 350,
                Description = "صندوق فاخر",
                Image = "gift2.jpg"
            }
        };

        public IActionResult Index()
        {
            // Session
            HttpContext.Session.SetString("UserName", "EngReem");
            HttpContext.Session.SetInt32("UserId", 105);

            ViewBag.CategoriesList = _categories;

            return View();
        }

        public IActionResult Products(int id, string? searchTerm)
        {
            var filtered = _products.Where(p => p.CategoryId == id).ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filtered = filtered
                    .Where(p => p.Name.Contains(searchTerm))
                    .ToList();
            }

            ViewBag.FilteredProducts = filtered;
            ViewBag.CategoryName = _categories[id];
            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CategoryId = id;

            return View();
        }

        public IActionResult Details(string name)
        {
            var product = _products.FirstOrDefault(p => p.Name == name);

            if (product == null)
                return NotFound();

            ViewBag.Product = product;

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}