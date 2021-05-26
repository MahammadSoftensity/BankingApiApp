using System.Threading.Tasks;
using BankingApp.DTOs;
using BankingApp.Helpers.Params;

namespace BankingApp.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserDto>> Register(UserParams userParams);
        Task<ServiceResult<UserDto>> Login(LoginParams userParams);
    }
}
