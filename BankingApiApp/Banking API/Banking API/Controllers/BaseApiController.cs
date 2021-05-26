using BankingApp.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Banking_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {
        public ActionResult<T> ReturnResult<T>(ServiceResult<T> serviceResult) 
        {
            if (serviceResult.Errors?.Count > 0)
            {
                return BadRequest(serviceResult.Errors);
            }

            return Ok(serviceResult);
        }
    }
}
