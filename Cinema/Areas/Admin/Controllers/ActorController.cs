using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(CD.ADMIN_AREA)]
    public class ActorController : Controller
    {
        private readonly IRepository<Actor> _actorRepo;
        public ActorService _actorService=new ActorService();

        public ActorController(IRepository<Actor> actorRepo)
        {
            _actorRepo = actorRepo;
            
        }

        public async Task<IActionResult> Index(string actorName, int page = 1)
        {
            var actors =await _actorRepo.GetAllAsync();
            //filter 
            if (actorName != null)
            {
                actors = actors.Where(c => c.Name.Contains(actorName));
                ViewBag.ActorName = actorName;
            }

            // pagination 
          
            return View(new ActorVM()
            {
                Actors = actors.AsEnumerable(),
               
                CurrentPage = page
            });
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateActorVM());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateActorVM createActorVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createActorVM);
            }
            var actor = new Actor()
            {
                Name = createActorVM.Name,
                Description = createActorVM.Description,
                Status = createActorVM.Status,

            };
            if (createActorVM.ImageFile != null && createActorVM.ImageFile.Length > 0)
            {
                var fileName = _actorService.SaveFile(createActorVM.ImageFile);
                actor.Logo = fileName;
            }
            //_context.actors.Add(actor);
            //_context.SaveChanges();
            await _actorRepo.CreateAsync(actor);
           await _actorRepo.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var actor = _context.actors.FirstOrDefault(c => c.Id == id);
            var actor =await _actorRepo.GetOneAsync(c => c.Id == id);
            if (actor is null)
            {
                return NotFound();
            }
            return View(actor);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Actor actor, IFormFile ImageFile)
        {
            //var actorInDb = _context.actors.AsNoTracking().FirstOrDefault(b => b.Id == actor.Id);
              var actorInDb =await _actorRepo.GetOneAsync(b => b.Id == actor.Id,IsTracking: false);


            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = _actorService.SaveFile(ImageFile);
                actor.Logo = fileName;
                _actorService.RemoveFile(actorInDb.Logo);
            }
            else
            {
                actor.Logo = actorInDb.Logo;
            }
            //_context.actors.Update(actor);
            //_context.SaveChanges();
            _actorRepo.Update(actor);
           await _actorRepo.CommitAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            //var actor = _context.actors.FirstOrDefault(c => c.Id == id);
            var actor =await _actorRepo.GetOneAsync(c => c.Id == id);
            if (actor is null)
            {
                return NotFound();
            }
            _actorService.RemoveFile(actor.Logo);
            //_context.actors.Remove(actor);
            //_context.SaveChanges();
            _actorRepo.Delete(actor);
             await _actorRepo.CommitAsync();
            return RedirectToAction(nameof(Index));

        }


    }
}
