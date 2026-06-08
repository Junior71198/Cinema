namespace Cinema.ViewModels
{
    public class MovieVM
    {
        public IEnumerable<Movie> Movies { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
