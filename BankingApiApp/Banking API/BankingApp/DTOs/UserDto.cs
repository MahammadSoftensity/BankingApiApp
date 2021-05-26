using System.Collections.Generic;

namespace BankingApp.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<AccountDto> Accounts { get; set; }
        public string Token { get; set; }
    }
}
