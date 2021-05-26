

using System.ComponentModel.DataAnnotations;

namespace BankingApp.Helpers.Params
{
    public class LoginParams
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
