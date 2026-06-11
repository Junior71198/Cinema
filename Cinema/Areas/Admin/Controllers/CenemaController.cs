using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(CD.ADMIN_AREA)]
    public class CenemaController : Controller
    {
        private readonly IRepository<Cenema> _cenemaRepo;
        private readonly CenemaService _CenemaService=new CenemaService();

        public CenemaController(IRepository<Cenema> cenemaRepo)
        {
            _cenemaRepo = cenemaRepo;
        }

        public async Task<IActionResult> Index(string CenemaName, int page = 1)
        {
            //var Cenemas = _context.cenemas.AsQueryable();
            var Cenemas =await _cenemaRepo.GetAllAsync();
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
        public async Task<IActionResult> Create(CreateCenemaVM createCenemaVM)
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
            //_context.cenemas.Add(Cenema);
            //_context.SaveChanges();
            await _cenemaRepo.CreateAsync(Cenema);
            await _cenemaRepo.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var Cenema = _context.cenemas.FirstOrDefault(c => c.Id == id);
            var Cenema =await _cenemaRepo.GetOneAsync(c => c.Id == id);
            if (Cenema is null)
            {
                return NotFound();
            }
            return View(Cenema);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Cenema Cenema, IFormFile ImageFile)
        {
            //var CenemaInDb = _context.cenemas.AsNoTracking().FirstOrDefault(b => b.Id == Cenema.Id);
                var CenemaInDb =await _cenemaRepo.GetOneAsync(b => b.Id == Cenema.Id,IsTracking:false);

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
            //_context.cenemas.Update(Cenema);
            //_context.SaveChanges();
            _cenemaRepo.Update(Cenema);
            await _cenemaRepo.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            //var Cenema = _context.cenemas.FirstOrDefault(c => c.Id == id);
            var Cenema =await _cenemaRepo.GetOneAsync(c => c.Id == id);
            if (Cenema is null)
            {
                return NotFound();
            }
            _CenemaService.RemoveFile(Cenema.Logo);
            //_context.cenemas.Remove(Cenema);
            //_context.SaveChanges();
            _cenemaRepo.Delete(Cenema);
            await _cenemaRepo.CommitAsync();
            return RedirectToAction(nameof(Index));

        }


    }
}
