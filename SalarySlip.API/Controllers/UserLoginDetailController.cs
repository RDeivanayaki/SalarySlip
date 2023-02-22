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

        [HttpPost]
        public IActionResult AddUserLoginDetail(Userlist user)
        {
            return Ok(new JsonResult(_userLoginDetailRepository.Login(user)));
        }
    }
}
