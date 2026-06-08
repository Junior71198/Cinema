namespace Cinema.ViewModels
{
    public class CenemaVM
    {
        public IEnumerable<Cenema> Cenemas { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
