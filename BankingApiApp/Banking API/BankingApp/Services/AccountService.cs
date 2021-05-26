using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BankingApp.DTOs;
using BankingApp.Helpers.Params;
using BankingApp.Interfaces;
using BankingEntities.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Services
{
    public class AccountService : IAccountService
    {
        readonly DataContext _dataContext;
        readonly IMapper _mapper;

        public AccountService(DataContext dataContext,IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResult<UserDto>> AddUserAccount(AccountParams accountParams, int userId)
        {
            var user = await _dataContext.Users
                .Include(x=>x.Accounts)
                .FirstOrDefaultAsync(x => x.Id == userId);

            user.Accounts.Add(new Account{ Sum = accountParams.Sum });

            await _dataContext.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Accounts = _mapper.Map<ICollection<AccountDto>>(user.Accounts),
            };

            return new ServiceResult<UserDto>(userDto);
        }

        public async Task<ServiceResult<UserDto>> TransferAmount(TransferAmountParams transferParams, int userId)
        {

            var user = await _dataContext.Users
                .Include(x=>x.Accounts)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null || user.Accounts.All(x => x.Id != transferParams.CurrentAccountId))
            {
                return new ServiceResult<UserDto>(new List<string> { "Account doesnt exist" });
            }

            var account = user.Accounts.First(x => x.Id == transferParams.CurrentAccountId);

            if (transferParams.Sum > account.Sum || transferParams.Sum <= 0)
            {
                return new ServiceResult<UserDto>(new List<string> { "Not enough amount on selected account" });
            }

            var transferAccount =  await _dataContext.Accounts.FirstOrDefaultAsync(x => x.Id == transferParams.TransferAccountId);

            if (transferAccount == null || transferParams.CurrentAccountId == transferParams.TransferAccountId)
            {
                return new ServiceResult<UserDto>(new List<string> { "Account doesnt exist" });
            }

            account.Sum -= transferParams.Sum;

            transferAccount.Sum += transferParams.Sum;

            await _dataContext.TransferHistories.AddAsync(new TransferHistory
            {
                ReceiveAccountId = transferAccount.Id,
                SendAccountId = account.Id,
                Sum = transferParams.Sum
            });

            await _dataContext.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = account.User.Id,
                Name = account.User.Name,
                Accounts = _mapper.Map<ICollection<AccountDto>>(user.Accounts),
            };

            return new ServiceResult<UserDto>(userDto);
        }

        public async Task<ServiceResult<ICollection<BalanceDto>>> GetUserBalance(int? accountId, int userId)
        {
            if (accountId == null)
            {
                var accounts = await _dataContext.Accounts.Where(x => x.UserId == userId).ToListAsync();

                return new ServiceResult<ICollection<BalanceDto>>(_mapper.Map<ICollection<BalanceDto>>(accounts)); 
            }

            var account = await _dataContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId && x.UserId == userId);

            if (account == null)
            {
                return new ServiceResult<ICollection<BalanceDto>>(new List<string> { "Not permission" });
            }

            return new ServiceResult<ICollection<BalanceDto>>(_mapper.Map<ICollection<BalanceDto>>(new List<Account> { account }));
        }

        public async Task<ServiceResult<ICollection<TransferHistoryDto>>> GetTransferHistory(int? accountId, int userId)
        {
            if (accountId == null)
            {
                var transferHistory = await _dataContext.TransferHistories
                    .Where(x => x.ReceiveAccount.UserId == userId || x.SendAccount.UserId == userId).ToListAsync();

                return new ServiceResult<ICollection<TransferHistoryDto>>(_mapper.Map<ICollection<TransferHistoryDto>>(transferHistory));  
            }

            var accountTransferHistory = await _dataContext.TransferHistories
                .Where(x => 
                    (x.SendAccount.UserId == userId || x.ReceiveAccount.UserId == userId) &&
                    (x.ReceiveAccountId == accountId || x.SendAccountId == accountId))
                .ToListAsync();


            return new ServiceResult<ICollection<TransferHistoryDto>>(_mapper.Map<ICollection<TransferHistoryDto>>(accountTransferHistory)); 
        }
    }
}
