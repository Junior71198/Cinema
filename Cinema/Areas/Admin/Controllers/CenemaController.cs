using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(CD.ADMIN_AREA)]
    public class CenemaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CenemaService _CenemaService;

        public CenemaController()
        {
            _context = new ApplicationDbContext();
            _CenemaService = new CenemaService();
        }

        public IActionResult Index(string CenemaName, int page = 1)
        {
            var Cenemas = _context.cenemas.AsQueryable();
            //filter 
            if (CenemaName != null)
            {
                Cenemas = Cenemas.Where(c => c.Name.Contains(CenemaName));
                ViewBag.CenemaName = CenemaName;
            }

            // pagination 
          
            return View(new CenemaVM()
            {
                Cenemas = Cenemas.AsEnumerable(),
               
                CurrentPage = page
            });
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateCenemaVM());
        }
        [HttpPost]
        public IActionResult Create(CreateCenemaVM createCenemaVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createCenemaVM);
            }
            var Cenema = new Cenema()
            {
                Name = createCenemaVM.Name,
                Description = createCenemaVM.Description,
                Status = createCenemaVM.Status,

            };
            if (createCenemaVM.ImageFile != null && createCenemaVM.ImageFile.Length > 0)
            {
                var fileName = _CenemaService.SaveFile(createCenemaVM.ImageFile);
                Cenema.Logo = fileName;
            }
            _context.cenemas.Add(Cenema);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Cenema = _context.cenemas.FirstOrDefault(c => c.Id == id);
            if (Cenema is null)
            {
                return NotFound();
            }
            return View(Cenema);
        }
        [HttpPost]
        public IActionResult Edit(Cenema Cenema, IFormFile ImageFile)
        {
            var CenemaInDb = _context.cenemas.AsNoTracking().FirstOrDefault(b => b.Id == Cenema.Id);


            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = _CenemaService.SaveFile(ImageFile);
                Cenema.Logo = fileName;
                _CenemaService.RemoveFile(CenemaInDb.Logo);
            }
            else
            {
                Cenema.Logo = CenemaInDb.Logo;
            }
            _context.cenemas.Update(Cenema);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var Cenema = _context.cenemas.FirstOrDefault(c => c.Id == id);
            if (Cenema is null)
            {
                return NotFound();
            }
            _CenemaService.RemoveFile(Cenema.Logo);
            _context.cenemas.Remove(Cenema);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }


    }
}
