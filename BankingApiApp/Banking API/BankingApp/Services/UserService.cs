using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
    public class UserService : IUserService
    {
        readonly DataContext _dataContext;
        readonly IMapper _mapper;
        readonly ITokenService _tokenService;

        public UserService(DataContext dataContext, IMapper mapper, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<ServiceResult<UserDto>> Login(LoginParams userParams)
        {
            var users = _dataContext.Users
                .Include(x => x.Accounts)
                .Where(x => x.Name == userParams.Name);

            if (!users.Any())
            {
                return new ServiceResult<UserDto>(new List<string>{ "Incorrect login or password" });
            }

            AppUser authUser = null;

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512(user.PassSalt);

                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userParams.Password));

                if (Encoding.UTF8.GetString(computeHash) == Encoding.UTF8.GetString(user.PassHash))
                {
                    authUser = user;
                    break;
                }
            }

            if (authUser == null)
            {
                return new ServiceResult<UserDto>(new List<string> { "Incorrect login or password" });
            }

            var userDto = new UserDto
            {
                Id = authUser.Id,
                Name = authUser.Name,
                Token = _tokenService.CreateToken(authUser),
                Accounts = _mapper.Map<ICollection<AccountDto>>(authUser.Accounts),
            };

            return new ServiceResult<UserDto>(userDto);
        }

        public async Task<ServiceResult<UserDto>> Register(UserParams userParams)
        {
            var user = new AppUser();

            using var hmac = new HMACSHA512();

            user.Name = userParams.Name;
            user.PassHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userParams.Password));
            user.PassSalt = hmac.Key;

            user.Accounts = new List<Account>
            {
                new Account{ Sum = userParams.Sum }
            };

            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            var userDto = new UserDto
            {
                Accounts = _mapper.Map<ICollection<AccountDto>>(user.Accounts),
                Name = user.Name,
                Id = user.Id,
                Token = _tokenService.CreateToken(user)
            };

            return new ServiceResult<UserDto>(userDto);
        }
    }
}
