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
        private readonly ApplicationDbContext _context;
        private readonly MovieService _movieService;

        public MovieController()
        {
            _context = new ApplicationDbContext();
            _movieService = new MovieService();
        }

        public IActionResult Index(MovieFilterVM movieFilterVM, int page = 1)
        {
            var movies = _context.movies.Include(m=>m.Category).Include(m=>m.Cinema).AsQueryable();
         
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
            ViewData["Categories"] =_context.categories.AsEnumerable();
            ViewData["Cenemas"] =_context.cenemas.AsEnumerable();

           
          
            return View(new MovieVM()
            {
                Movies = movies.AsEnumerable(),
               
                CurrentPage = page
            });
        }
        [HttpGet]
        public IActionResult Create()
        {
            var categories = _context.categories.AsEnumerable();   
            var cinemas = _context.cenemas.AsEnumerable();
            return View(new CreateMovieVM()
            {
                Categories = categories,
                Cenemas = cinemas
            });
        }
      
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile ImageFile, List<IFormFile> SubImageFiles)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = _movieService.SaveFile(ImageFile);
                movie.MainImg = fileName;
            }
            var savedMovie = _context.movies.Add(movie);
            _context.SaveChanges();

            if (SubImageFiles != null && SubImageFiles.Count > 0)
            {
                foreach (var image in SubImageFiles)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileName = _movieService.SaveFile(image, MovieImageType.Sub);
                        _context.movieSubImages.Add(new MovieSubImage()
                        {
                            Img = fileName,
                            MovieId = savedMovie.Entity.Id
                        });
                    }
                }
                _context.SaveChanges();
            }
           
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.movies.FirstOrDefault(c => c.Id == id);
            if (movie is null)
            {
                return NotFound();
            }
            var categories = _context.categories.AsEnumerable();
           
            var cenemas = _context.cenemas.AsEnumerable();
            return View(new CreateMovieVM()
            {
                Movie = movie,
                Categories = categories,
                Cenemas = cenemas,
                MovieSubImages = _context.movieSubImages.Where(ms => ms.MovieId == id),
              
            });
        }
        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile ImageFile, List<IFormFile> SubImageFiles, List<string> Colors)
        {
            var movieInDb = _context.movies.AsNoTracking().FirstOrDefault(b => b.Id == movie.Id);

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
            _context.movies.Update(movie);
            _context.SaveChanges();

            if (SubImageFiles != null && SubImageFiles.Count > 0)
            {
                var oldMovieSubImages = _context.movieSubImages.Where(ms => ms.MovieId == movie.Id);
                
                _context.movieSubImages.RemoveRange(oldMovieSubImages);
               
                foreach (var item in oldMovieSubImages)
                {
                    _movieService.RemoveFile(item.Img, MovieImageType.Sub);
                }
                foreach (var image in SubImageFiles)
                {
                    if (image != null && image.Length > 0)
                    {
                        
                        var fileName = _movieService.SaveFile(image, MovieImageType.Sub);
                         
                        _context.movieSubImages.Add(new MovieSubImage()
                        {
                            Img = fileName,
                            MovieId = movie.Id
                        });
                    }
                }
                _context.SaveChanges();
            }
         
        
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var movie = _context.movies.FirstOrDefault(c => c.Id == id);
            if (movie is null)
            {
                return NotFound();
            }
            _movieService.RemoveFile(movie.MainImg);
            _context.movies.Remove(movie);
            var movieSubImages = _context.movieSubImages.Where(ms => ms.MovieId == movie.Id);
            foreach (var item in movieSubImages)
            {
                _movieService.RemoveFile(item.Img, MovieImageType.Sub);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }


    }
}
