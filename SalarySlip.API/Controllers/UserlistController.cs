using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalarySlip.API.Repositories;
using SalarySlip.API.Models.Domain;

namespace SalarySlip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserlistController : ControllerBase
    {
        private readonly IUserlistRepository _userlistRepository;
        public UserlistController(IUserlistRepository userlistRepository)
        {
            _userlistRepository = userlistRepository;
        }

        [HttpPost("register")]
        public IActionResult Register(Userlist user)
        {
            string msg = _userlistRepository.Add(user);
            if (msg == "Registration successful")
            {
                return Ok(new
                {
                    Message = msg
                });
            }
            else
            {
                return BadRequest(msg);
            }
        }
    }
}
