using System.ComponentModel.DataAnnotations;

namespace SalarySlip.API.Models.Domain
{
    public class SalaryForMonthYear
    {
        [Key]
        public int MonthlySalaryId { get; set; }
        public string Month { get; set; } = "";
        public int Year { get; set; }
        public DateTime DateOfUpload { get; set; }
        public string UploadedFilename { get; set; } = "";

    }

}
