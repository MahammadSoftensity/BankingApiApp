using AutoMapper;
using BankingApp.DTOs;
using BankingEntities.Entities;

namespace BankingApp.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Account, AccountDto>();
            CreateMap<Account, BalanceDto>();
            CreateMap<TransferHistory, TransferHistoryDto>();
        }
    }
}
