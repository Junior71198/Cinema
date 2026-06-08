using System.ComponentModel.DataAnnotations;

namespace Cinema.ViewModels
{
    public class CreateActorVM
    {
     
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public bool Status { get; set; }
        public  IFormFile ImageFile { get; set; }

    }
}
