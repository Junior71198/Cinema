namespace Cinema.Models
{
    public class Cenema
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string Logo { get; set; }

        public bool Status { get; set; }
    }
}
