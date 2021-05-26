using System.ComponentModel.DataAnnotations;

namespace BankingApp.Helpers.Params
{
    public class UserParams
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Sum { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
