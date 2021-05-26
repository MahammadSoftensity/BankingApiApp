using System.Collections.Generic;
using System.Threading.Tasks;
using BankingApp.DTOs;
using BankingApp.Extensions;
using BankingApp.Helpers.Params;
using BankingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking_API.Controllers
{
    [Authorize]
    public class AccountController : BaseApiController
    {
        readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("AddUserAccount")]
        public async Task<ActionResult<UserDto>> AddUserAccount(AccountParams userParams)
        {
            return ReturnResult(await _accountService.AddUserAccount(userParams, User.GetUserId()));
        }

        [HttpPost("TransferAmount")]
        public async Task<ActionResult<UserDto>> TransferAmount(TransferAmountParams transferAmount)
        {
            return ReturnResult(await _accountService.TransferAmount(transferAmount, User.GetUserId()));
        }

        [HttpGet("GetUserBalance")]
        public async Task<ActionResult<ICollection<BalanceDto>>> GetUserBalance(int? accountId)
        {
            return ReturnResult(await _accountService.GetUserBalance(accountId, User.GetUserId()));
        }

        [HttpGet("GetTransferHistory")]
        public async Task<ActionResult<ICollection<TransferHistoryDto>>> GetTransferHistory(int? accountId)
        {
            return ReturnResult(await _accountService.GetTransferHistory(accountId, User.GetUserId()));
        }
    }
}
