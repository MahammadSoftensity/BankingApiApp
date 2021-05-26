
namespace BankingApp.DTOs
{
    public class TransferHistoryDto
    {
        public int SendAccountId { get; set; }
        public int ReceiveAccountId { get; set; }
        public double Sum { get; set; }
    }
}
