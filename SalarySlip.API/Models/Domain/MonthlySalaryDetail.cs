using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SalarySlip.API.Models.Domain
{
    public class MonthlySalaryDetail
    {
        public int MonthlySalaryId { get; set; }
        public int SINo { get; set; }
        public string SalaryType { get; set; } = "";
        public string BranchCode { get; set; } = "";
        public string EmployeeNo { get; set; } = "";
        public string EmployeeName { get; set; } = "";
        public string Department { get; set; } = "";
        public string Designation { get; set; } = "";
        public string DateofJoining { get; set; } = ""; 
        public double MonthlyGross { get; set; }
        public double Workdays { get; set; }
        public double ExtraDays { get; set; }
        public double TotalDays { get; set; }
        public int Gross { get; set; }
        public double PF { get; set; }
        public double ESI { get; set; }
        public double Loan { get; set; }
        public double SalaryAdvance { get; set; }
        public double Shortage { get; set; }
        public double TotalDeductions { get; set; }
        public int NetPay { get; set; }
        //public string NetPay { get; set; }

    }
}
