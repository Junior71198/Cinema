namespace Cinema.Repository
{
    public class MovieSubImageRepo : Repository<MovieSubImage> , IMovieSubImageRepo
    {
        private readonly ApplicationDbContext _context;

        public MovieSubImageRepo()
        {
            _context = new ApplicationDbContext();
        }

        public void DeleteRange(IEnumerable<MovieSubImage> movieSubImages)
        {
            _context.movieSubImages.RemoveRange(movieSubImages);
        }
    }
}
