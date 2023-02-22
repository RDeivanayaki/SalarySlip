using Microsoft.AspNetCore.Mvc;
using SalarySlip.API.Repositories;

namespace SalarySlip.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonthlistController : Controller
    {
        private readonly IMonthlistRepository _monthlistRepository;
        
        public MonthlistController(IMonthlistRepository monthlistRepository)
        {
            _monthlistRepository = monthlistRepository;
        }

        [HttpGet]

        public IActionResult GetAllMonths()
        {
            var monthlist = _monthlistRepository.GetAll();
            return Ok(monthlist);
        }
        
    }
}
