namespace Cinema.Repository
{
    public interface IMovieSubImageRepo : IRepository<MovieSubImage>
    {
        public void DeleteRange(IEnumerable<MovieSubImage> movieSubImages);
        

    }
}
