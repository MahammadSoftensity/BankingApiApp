using System.Collections.Generic;

namespace BankingEntities.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] PassHash { get; set; }
        public byte[] PassSalt { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
