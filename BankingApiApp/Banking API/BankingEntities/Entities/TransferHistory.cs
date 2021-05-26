

namespace BankingEntities.Entities
{
    public class TransferHistory
    {
        public int Id { get; set; }
        public int SendAccountId { get; set; }
        public Account SendAccount { get; set; }
        public int ReceiveAccountId { get; set; }
        public Account ReceiveAccount { get; set; }
        public double Sum { get; set; }
    }
}
