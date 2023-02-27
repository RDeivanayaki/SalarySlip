using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalarySlip.API.Repositories;

namespace SalarySlip.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class YearlistController : Controller
    {
        private readonly IYearlistRepository _yearlistRepository;
        public YearlistController(IYearlistRepository yearlistRepository)
        {
            _yearlistRepository = yearlistRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllYears()
        {
            var yearlist = _yearlistRepository.GetAll();
            return Ok(yearlist);
        }
       
    }
}
