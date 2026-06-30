using Microsoft.AspNetCore.Mvc;

namespace mini_store.Controllers;

public class HomeController : Controller
{
    private static dynamic[] _categories =
    {
        new { Id = 0, Name = "ورود طبيعية", Icon = "fa-solid fa-seedling" },
        new { Id = 1, Name = "باقات ورد", Icon = "fa-solid fa-gift" },
        new { Id = 2, Name = "هدايا ورد", Icon = "fa-solid fa-heart" }
    };

    private static dynamic[] _products =
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
            Description = "وردأنيق للمناسبات ومختار بعناية ليعبر عن اصدق المشاعر",
            Image = "rose2.jpg"
        },

        new
        {
            CategoryId = 1,
            Name = "بوكيه ليلي فاخر",
            Price = 250,
            Description = "بوكيه ورد فاخر ومميز",
            Image = "bouquet1.jpg"
        },

        new
        {
            CategoryId = 1,
            Name = "بوكيه ورد وردي",
            Price = 220,
            Description = "بوكيه أنيق باللون الوردي",
            Image = "bouquet2.jpg"
        },

        new
        {
            CategoryId = 2,
            Name = "هدية ورد وشوكولاتة",
            Price = 300,
            Description = "هدية مميزة مع الشوكولاتة تترك اثرا جميلا في القلب",
            Image = "gift1.jpg"
        },

        new
        {
            CategoryId = 2,
            Name = " ورد مع شوكولاتة فاخر",
            Price = 350,
            Description = "صندوق ورد فاخر مع الشوكولاتة لتناسب اجمل المناسبات",
            Image = "gift2.jpg"
        }
    };

    public IActionResult Index()
    {
        ViewBag.CategoriesList = _categories;
        return View();
    }

    public IActionResult Products(int id, string searchTerm)
{
    var filtered = _products
        .Where(p => p.CategoryId == id)
        .ToList();

    if (!string.IsNullOrEmpty(searchTerm))
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
    var product = _products
        .FirstOrDefault(p => p.Name == name);

    if (product == null)
    {
        return NotFound();
    }

    ViewBag.Product = product;

    return View();
}

public IActionResult Login()
{
    return View();
}

[HttpPost]
public IActionResult Login(string userName, string password)
{
    ViewBag.Message = "Login successfully";
    ViewBag.UserName = userName;

    return RedirectToAction("Index", "Home");
}
public IActionResult About()
{
    return View();
}
}
