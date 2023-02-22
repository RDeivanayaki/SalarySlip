using SalarySlip.API.Models.Domain;
using System.Data;
using System.Data.SqlClient;

namespace SalarySlip.API.Repositories
{
    public class YearlistRepository: IYearlistRepository
    {
        SqlConnection con;
        private readonly IConfiguration _iConfiguration;

        public YearlistRepository(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;
            con = new SqlConnection(iConfiguration.GetConnectionString("SalarySlip"));
        }

        public IEnumerable<Yearlist> GetAll()
        {
            SqlDataAdapter da = new SqlDataAdapter("usp_GetYears", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            List<Yearlist> yearlist = new List<Yearlist>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Yearlist year = new Yearlist();
                    year.Year = dt.Rows[i]["Year"].ToString();
                    yearlist.Add(year);
                }

            }

            return yearlist;
        }
    }


}
