using Microsoft.AspNetCore.Mvc;
using SalarySlip.API.Repositories;

namespace SalarySlip.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonthlySalaryDetailController : Controller
    {
        private readonly IMonthlySalaryDetailRepository _monthlySalaryDetailRepository;
        private readonly IWebHostEnvironment _env;

        public MonthlySalaryDetailController(IMonthlySalaryDetailRepository monthlySalaryDetailRepository, IWebHostEnvironment env)
        {
            _monthlySalaryDetailRepository = monthlySalaryDetailRepository;
            _env = env;
        }

        [Route("Upload")]
        [HttpPost]
        public IActionResult Upload()
        {
            try
            {
                var httpRequest = Request.Form;
                var uploadedFile = httpRequest.Files[0];
                string filename = uploadedFile.FileName;
                var physicalPath = _env.ContentRootPath + "Upload\\" + filename;
                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    uploadedFile.CopyTo(stream);
                }
                
                return Json(filename);
            }
            catch (Exception)
            {
                return Json("Failed");
            }

        }
    }
}
