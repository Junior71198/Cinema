namespace Cinema.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        public string? Description { get; set; }

        public string MainImg { get; set; } 

        public bool Status { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }
        
        public double Rate { get; set; }

        public int CategoryId { get; set; }

        public int CinemaId { get; set; }
        public DateTime DateTime { get; set; }

        public  Cenema Cinema { get; set; } 

        public Category Category { get; set; }

        public  ICollection<MovieSubImage> MovieSubImage { get; set; }
    }
}
