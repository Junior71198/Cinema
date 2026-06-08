using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(CD.ADMIN_AREA)]

    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController()
        {
            _context = new ApplicationDbContext();
        }

        public IActionResult Index(string categoryName)
        {
            var categories = _context.categories.AsQueryable();

            if (!string.IsNullOrEmpty(categoryName))
            {
                categories = categories.Where(c => c.Name.Contains(categoryName));
                ViewBag.CategoryName = categoryName;
            }

            return View(categories.AsEnumerable());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            _context.categories.Add(category);
            _context.SaveChanges();
            //Response.Cookies.Append("Success_Notification" , "Category Careted Successfully");
            TempData["Success_Notification"] = "Category Careted Successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.categories.FirstOrDefault(c => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            _context.categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var category = _context.categories.FirstOrDefault(c => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            _context.categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
