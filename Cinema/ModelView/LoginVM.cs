using System.ComponentModel.DataAnnotations;

namespace Cinema.ModelView
{
    public class LoginVM
    {
        int Id { get; set; }

        public string UserNameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
