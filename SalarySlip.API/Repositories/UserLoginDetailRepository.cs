using System.Data;
using System.Data.SqlClient;
using SalarySlip.API.Models.Domain;

namespace SalarySlip.API.Repositories
{
    public class UserLoginDetailRepository: IUserLoginDetailRepository
    {
        SqlConnection con;
        private readonly IConfiguration _iconfiguration;
        public UserLoginDetailRepository(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            con = new SqlConnection(_iconfiguration.GetConnectionString("SalarySlip"));
        }

        public string Login(Userlist user)
        {
            SqlCommand cmd = new SqlCommand("usp_AddUserLoginDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@UserName", user.UserName);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.Add("@UserId",SqlDbType.Int);
            cmd.Parameters["@UserId"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Result", SqlDbType.Bit);
            cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
            string errorMsg = "";
            
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                bool result = Convert.ToBoolean(cmd.Parameters["@Result"].Value);
                Console.WriteLine("The result is : " + result.ToString());
                if (result)
                    errorMsg = "Success";
                else
                    errorMsg = "User should signup";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                errorMsg = ex.Message;
            }
            finally
            {
                con.Close();
            }
            
            return errorMsg;
        }
    }
}
