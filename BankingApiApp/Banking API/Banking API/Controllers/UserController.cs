using System.Threading.Tasks;
using BankingApp.DTOs;
using BankingApp.Helpers.Params;
using BankingApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking_API.Controllers
{
    public class UserController : BaseApiController
    {
        readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Register new user with name, password and initial sum
        /// </summary>
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(UserParams userParams)
        {
            return ReturnResult(await _userService.Register(userParams));
        }


        /// <summary>
        /// Login user
        /// </summary>
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginParams loginParams)
        {
            return ReturnResult(await _userService.Login(loginParams));
        }
    }
}
