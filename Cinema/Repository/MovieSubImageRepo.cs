namespace Cinema.Repository
{
    public class MovieSubImageRepo : Repository<MovieSubImage> , IMovieSubImageRepo
    {
        public MovieSubImageRepo(ApplicationDbContext context) : base(context)
        {
        }

   

        public void DeleteRange(IEnumerable<MovieSubImage> movieSubImages)
        {
            _context.movieSubImages.RemoveRange(movieSubImages);
        }
    }
}
