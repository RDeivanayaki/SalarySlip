
using System.Data.SqlClient;
using System.Data;
using SalarySlip.API.Models.Domain;
using System.Collections.Generic;
using ExcelDataReader;

namespace SalarySlip.API.Repositories
{
    public class MonthlySalaryDetailRepository : IMonthlySalaryDetailRepository
    {
        SqlConnection con;
        private readonly IConfiguration _iConfiguration;
        public MonthlySalaryDetailRepository(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
            con = new SqlConnection(_iConfiguration.GetConnectionString("SalarySlip"));
        }
    }
}
