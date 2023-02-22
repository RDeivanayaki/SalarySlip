namespace SalarySlip.API.Models.Domain
{
    public class SalarySlipDetail
    {
        public string BranchCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNo { get; set; }
        public int Salary { get; set; }
        //public string Salary { get; set; }
        public int MonthlySalaryId { get; set; }
    }
}
