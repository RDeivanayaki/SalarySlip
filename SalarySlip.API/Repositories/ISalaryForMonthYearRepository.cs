using Microsoft.AspNetCore.Mvc;
using SalarySlip.API.Models.Domain;

namespace SalarySlip.API.Repositories
{
    public interface ISalaryForMonthYearRepository
    {
        string Add(SalaryForMonthYear salary);
        string ReadExFile(string fileName,int MonthlySalaryId, SalaryForMonthYear salary);
        string DataInsertionForSalaryForMonthYear(int MonthlySalaryId, SalaryForMonthYear salary);
        void ExcelFileDataInsertion(List<MonthlySalaryDetail> salarydet);
        void CreateSalarySlip(List<MonthlySalaryDetail> salarydet);
        List<SalarySlipDetail> GeneratePdf();
        string GetMonth();
    }
}
