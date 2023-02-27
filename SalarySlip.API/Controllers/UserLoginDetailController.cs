using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalarySlip.API.Models.Domain;
using SalarySlip.API.Repositories;

namespace SalarySlip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginDetailController : ControllerBase
    {
        private readonly IUserLoginDetailRepository _userLoginDetailRepository;
        public UserLoginDetailController(IUserLoginDetailRepository userLoginDetailRepository)
        {
            _userLoginDetailRepository = userLoginDetailRepository;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] Userlist user)
        {
            string msg = _userLoginDetailRepository.Login(user);
            if (msg == "Login Success")
                return Ok(new
                {
                    Token = user.Token,
                    Message = msg
                });
            else
            {
               return BadRequest(msg);
            }
        }
    }
}
