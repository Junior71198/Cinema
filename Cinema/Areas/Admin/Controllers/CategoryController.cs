using Cinema.Models;
using Cinema.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(CD.ADMIN_AREA)]

    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepo;

        public CategoryController(IRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index(string categoryName)
        {
            //var categories = _context.categories.AsQueryable();
            var categories = await _categoryRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(categoryName))
            {
                categories = categories.Where(c => c.Name.Contains(categoryName));
              
                
                ViewBag.CategoryName = categoryName;
            }

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            //_context.categories.Add(category);
           await _categoryRepo.CreateAsync(category);
            //_context.SaveChanges();
            await _categoryRepo.CommitAsync();
            //Response.Cookies.Append("Success_Notification" , "Category Careted Successfully");
            TempData["Success_Notification"] = "Category Careted Successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var category = _context.categories.FirstOrDefault(c => c.Id == id);
            var category =await _categoryRepo.GetOneAsync(c => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            //_context.categories.Update(category);
            _categoryRepo.Update(category);
            //_context.SaveChanges();
            await _categoryRepo.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            //var category = _context.categories.FirstOrDefault(c => c.Id == id);
            var category =await _categoryRepo.GetOneAsync(c => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            //_context.categories.Remove(category);
            _categoryRepo.Delete(category);
            //_context.SaveChanges();
             await _categoryRepo.CommitAsync();
            return RedirectToAction(nameof(Index));

        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
