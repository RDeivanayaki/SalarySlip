using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using SalarySlip.API.Models.Domain;
using SalarySlip.API.Repositories;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Microsoft.AspNetCore.Authorization;

namespace SalarySlip.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaryForMonthYearController : Controller
    {
        private readonly ISalaryForMonthYearRepository _salaryForMonthYearRepository;
        private readonly IWebHostEnvironment _env;

        public SalaryForMonthYearController(ISalaryForMonthYearRepository salaryForMonthYearRepository, IWebHostEnvironment env)
        {
            _salaryForMonthYearRepository = salaryForMonthYearRepository;
            _env = env;
        }
                
        [HttpPost]
        public IActionResult AddSalary(SalaryForMonthYear salary)
        {
            string msg = _salaryForMonthYearRepository.Add(salary);
            
            return Ok(Json(msg));
        }

        [Authorize]
        [HttpGet("generatepdf")]
        public IActionResult GeneratePDF()
        {
            string month = _salaryForMonthYearRepository.GetMonth(); // "December";
            List<SalarySlipDetail> ssd = _salaryForMonthYearRepository.GeneratePdf();
            if (ssd.Count > 0)
            {
                var document = new PdfDocument();
                int employeeCount = 0;
                string HtmlContent = "";
                int totalEmployeeCount = 0;
                foreach (var sd in ssd)
                {
                    HtmlContent += "<h4 font-size:15px>&nbsp; &nbsp;" + sd.BranchCode + "</h4>";
                    HtmlContent += "<div style='width:100%; text-align:center;'>";
                    HtmlContent += "<h4 font-size:15px><b>Salary Slip</b></h4>";
                    HtmlContent += "</div>"; 
                    HtmlContent += "<h5 font-size:11px> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; " + month + " Month </h5>";
                    HtmlContent += "<h6 font-size:11px> &nbsp; &nbsp; &nbsp; Branch : " + sd.BranchCode + "</h6>";
                    HtmlContent += "<h6 font-size:11px> &nbsp; &nbsp; &nbsp; Name : " + sd.EmployeeName + "</h6>";
                    HtmlContent += "<h6 font-size:11px> &nbsp; &nbsp; &nbsp; ID NO : " + sd.EmployeeNo + "</h6>";
                    //if(sd.Salary == 0)
                    //    HtmlContent += "<h6 font-size:11px> &nbsp; &nbsp; &nbsp; Salary : </h6>";
                    //else
                        HtmlContent += "<h6 font-size:11px> &nbsp; &nbsp; &nbsp; Salary : " + sd.Salary + "</h6>";
                    //HtmlContent += "<br>";
                    //HtmlContent += "<br>";
                    //HtmlContent += "<br>";
                    HtmlContent += "<h5> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; HR &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; GM &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; MD</h5>";
                                        
                    employeeCount++;
                    totalEmployeeCount++;
                    //if (employeeCount <= 2)
                    //  HtmlContent += "<hr>";
                    if (employeeCount == 3)
                    {
                        PdfGenerator.AddPdfPages(document, HtmlContent, PageSize.A4);
                        employeeCount = 0;
                        HtmlContent = "";
                        
                    }
                    else if(totalEmployeeCount == ssd.Count)
                    {
                        PdfGenerator.AddPdfPages(document, HtmlContent, PageSize.A4);
                        employeeCount = 0;
                        HtmlContent = "";
                        totalEmployeeCount = 0;
                    }
                }
                byte[]? response = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    document.Save(ms);
                    response = ms.ToArray();
                }
                string Filename = "Salaryof" + month.Replace(" ", "") + ".pdf";

                return File(response, "application/pdf", Filename);
            }
            else
                return Json("No Data Found!");

        }
    }
}
