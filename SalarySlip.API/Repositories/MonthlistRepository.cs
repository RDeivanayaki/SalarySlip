using Microsoft.AspNetCore.Mvc;
using SalarySlip.API.Models.Domain;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Serialization;

namespace SalarySlip.API.Repositories
{
    public class MonthlistRepository : IMonthlistRepository 
    {
        SqlConnection con;
        private readonly IConfiguration _iConfiguration;
        public MonthlistRepository(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
            con = new SqlConnection(iConfiguration.GetConnectionString("SalarySlip"));
        }

        public IEnumerable<Monthlist> GetAll()
        {
            SqlDataAdapter da = new SqlDataAdapter("usp_GetMonths", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            List<Monthlist> monthlist = new List<Monthlist>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Monthlist month = new Monthlist();
                    month.Month = dt.Rows[i]["Month"].ToString();
                    monthlist.Add(month);
                }

            }

            return monthlist;
        }
    }
}
