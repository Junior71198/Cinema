using System.ComponentModel.DataAnnotations;

namespace Cinema.ViewModels
{
    public class CreateMovieVM
    {
        public Movie Movie { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Cenema> Cenemas { get; set; }

        public IEnumerable<Actor> Actors { get; set; }
        
        public IEnumerable<MovieSubImage> MovieSubImages { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int categoryId { get; set; }
        public int cenemaId { get; set; }
        public  IFormFile ImageFile { get; set; }
        public List<IFormFile> SubImageFiles { get; set; }
    }
}
