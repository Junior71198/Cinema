using Cinema.Models;
using Cinema.ModelView;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Cinema.Services.MovieService;

namespace Cinema.Areas.Admin.Controllers
{
    [Area(CD.ADMIN_AREA)]
    public class MovieController : Controller
    {
        private readonly IRepository<Movie> _movieRepo;
        private readonly IRepository<Cenema> _cenemaRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Actor> _actorRepo;
        private readonly IMovieSubImageRepo _movieSubImageRepo;




        private readonly MovieService _movieService=new MovieService();

        public MovieController(IRepository<Movie> movieRepo, IRepository<Cenema> cenemaRepo, IRepository<Category> categoryRepo, IRepository<Actor> actorRepo, IMovieSubImageRepo movieSubImageRepo)
        {
            _movieRepo = movieRepo;
            _cenemaRepo = cenemaRepo;
            _categoryRepo = categoryRepo;
            _actorRepo = actorRepo;
            _movieSubImageRepo = movieSubImageRepo;
        }

        public async Task<IActionResult> Index(MovieFilterVM movieFilterVM, int page = 1)
        {
            //var movies = _context.movies.Include(m=>m.Category).Include(m=>m.Cinema).AsQueryable();
            var movies =await _movieRepo.GetAllAsync(includes:[m => m.Category, m => m.Cinema]);

            if (movieFilterVM.movieName != null)
            {
                movies = movies.Where(c => c.Name.Contains(movieFilterVM.movieName));
                ViewBag.MovieName = movieFilterVM.movieName;
            }
            if (movieFilterVM.minPrice > 0)
            {
                movies = movies.Where(c => c.Price >= movieFilterVM.minPrice);
                ViewBag.MinPrice = movieFilterVM.minPrice;
            }
            if (movieFilterVM.maxPrice > 0)
            {
                movies = movies.Where(c => c.Price <= movieFilterVM.maxPrice);
                ViewBag.MaxPrice=movieFilterVM.maxPrice;
            }
            if (movieFilterVM.categoryId > 0)
            {
                movies = movies.Where(c => c.CategoryId == movieFilterVM.categoryId);
                ViewBag.CategoryId=movieFilterVM.categoryId;
            }
            if (movieFilterVM.cinemaId > 0)
            {
                movies = movies.Where(c => c.CinemaId == movieFilterVM.cinemaId);
                ViewBag.CinemaId=movieFilterVM.cinemaId;
            }
            //ViewData["Categories"] =_context.categories.AsEnumerable();
            ViewData["Categories"] =await _categoryRepo.GetAllAsync();
            //ViewData["Cenemas"] =_context.cenemas.AsEnumerable();
            ViewData["Cenemas"] =await _cenemaRepo.GetAllAsync();




            return View(new MovieVM()
            {
                Movies = movies.AsEnumerable(),
               
                CurrentPage = page
            });
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //var categories = _context.categories.AsEnumerable();   
            //var cinemas = _context.cenemas.AsEnumerable();
            var categories =await _categoryRepo.GetAllAsync();
            var cinemas = await _cenemaRepo.GetAllAsync();
            return View(new CreateMovieVM()
            {
                Categories = categories,
                Cenemas = cinemas
            });
        }
      
        [HttpPost]
        public async Task<IActionResult> Create(Movie movie, IFormFile ImageFile, List<IFormFile> SubImageFiles)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = _movieService.SaveFile(ImageFile);
                movie.MainImg = fileName;
            }
            //var savedMovie = _context.movies.Add(movie);
            //_context.SaveChanges();
            var savedMovie=await _movieRepo.CreateAsync(movie);
           await _movieRepo.CommitAsync();

            if (SubImageFiles != null && SubImageFiles.Count > 0)
            {
                foreach (var image in SubImageFiles)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileName = _movieService.SaveFile(image, MovieImageType.Sub);
                        //_context.movieSubImages.Add(new MovieSubImage()
                        //{
                        //    Img = fileName,
                        //    MovieId = savedMovie.Entity.Id
                        //});
                       await _movieSubImageRepo.CreateAsync(new()
                        {
                            Img = fileName,
                            MovieId = savedMovie.Entity.Id
                        });
                    }
                }
                //_context.SaveChanges();
               await _movieSubImageRepo.CommitAsync();
            }
           
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var movie = _context.movies.FirstOrDefault(c => c.Id == id);
            var movie =await _movieRepo.GetOneAsync(c => c.Id == id);
            if (movie is null)
            {
                return NotFound();
            }
            //var categories = _context.categories.AsEnumerable();
            var categories = await _categoryRepo.GetAllAsync();
            var cenemas = await _cenemaRepo.GetAllAsync();
            //var cenemas = _context.cenemas.AsEnumerable();
            return View(new CreateMovieVM()
            {
                Movie = movie,
                Categories = categories,
                Cenemas = cenemas,
                //First
                //MovieSubImages = _context.movieSubImages.Where(ms => ms.MovieId == id),
                MovieSubImages = await _movieSubImageRepo.GetAllAsync(ms => ms.MovieId == id)

            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Movie movie, IFormFile ImageFile, List<IFormFile> SubImageFiles, List<string> Colors)
        {
            //var movieInDb = _context.movies.AsNoTracking().FirstOrDefault(b => b.Id == movie.Id);
            var movieInDb =await _movieRepo.GetOneAsync(b => b.Id == movie.Id , IsTracking: false);
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = _movieService.SaveFile(ImageFile);
                movie.MainImg = fileName;
                _movieService.RemoveFile(movieInDb.MainImg);
            }
            else
            {
                movie.MainImg = movieInDb.MainImg;
            }
            //_context.movies.Update(movie);
            //_context.SaveChanges();
            _movieRepo.Update(movie);
            await _movieRepo.CommitAsync();

            if (SubImageFiles != null && SubImageFiles.Count > 0)
            {
                //var oldMovieSubImages = _context.movieSubImages.Where(ms => ms.MovieId == movie.Id);
                var oldMovieSubImages = await _movieSubImageRepo.GetAllAsync(ms => ms.MovieId == movie.Id);

                //_context.movieSubImages.RemoveRange(oldMovieSubImages);
                _movieSubImageRepo.DeleteRange(oldMovieSubImages);

                foreach (var item in oldMovieSubImages)
                {
                    _movieService.RemoveFile(item.Img, MovieImageType.Sub);
                }
                foreach (var image in SubImageFiles)
                {
                    if (image != null && image.Length > 0)
                    {
                        
                        var fileName = _movieService.SaveFile(image, MovieImageType.Sub);
                         
                        await _movieSubImageRepo.CreateAsync(new()
                        {
                            Img = fileName,
                            MovieId = movie.Id
                        });
                        //_context.movieSubImages.Add(new MovieSubImage()
                        //{
                        //    Img = fileName,
                        //    MovieId = movie.Id
                        //});
                    }
                }
                await _movieSubImageRepo.CommitAsync();
            }
         
        
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            //var movie = _context.movies.FirstOrDefault(c => c.Id == id);
            var movie =await _movieRepo.GetOneAsync(c => c.Id == id);
            if (movie is null)
            {
                return NotFound();
            }
            _movieService.RemoveFile(movie.MainImg);
            //_context.movies.Remove(movie);
            _movieRepo.Delete(movie);
            //var movieSubImages = _context.movieSubImages.Where(ms => ms.MovieId == movie.Id);
            var movieSubImages = await _movieSubImageRepo.GetAllAsync(ms => ms.MovieId == movie.Id);
            foreach (var item in movieSubImages)
            {
                _movieService.RemoveFile(item.Img, MovieImageType.Sub);
            }

            await _movieSubImageRepo.CommitAsync();
            return RedirectToAction(nameof(Index));

        }


    }
}
