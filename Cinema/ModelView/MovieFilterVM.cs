namespace Cinema.ModelView
{
    public class MovieFilterVM
    {
        public string movieName { get; set; }

        public decimal minPrice { get; set; }
        public decimal maxPrice { get; set; }

        public int categoryId { get; set; }

        public int cinemaId { get; set; }


    }
}
