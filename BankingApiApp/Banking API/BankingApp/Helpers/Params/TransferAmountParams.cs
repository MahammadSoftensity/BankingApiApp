

namespace BankingApp.Helpers.Params
{
    public class TransferAmountParams
    {
        public int CurrentAccountId { get; set; }
        public int TransferAccountId { get; set; }
        public double Sum { get; set; }
    }
}
