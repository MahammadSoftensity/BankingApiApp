using System.Collections.Generic;
using System.Threading.Tasks;
using BankingApp.DTOs;
using BankingApp.Helpers.Params;

namespace BankingApp.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<UserDto>> AddUserAccount(AccountParams accountParams, int userId);
        Task<ServiceResult<UserDto>> TransferAmount(TransferAmountParams transferParams,int userId);
        Task<ServiceResult<ICollection<BalanceDto>>> GetUserBalance(int userId, int? accountId = null);
        Task<ServiceResult<ICollection<TransferHistoryDto>>> GetTransferHistory(int userId, int? accountId = null);
    }
}
