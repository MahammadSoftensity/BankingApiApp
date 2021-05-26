namespace BankingEntities.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public double Sum { get; set; }
    }
}
