using Microsoft.VisualBasic;
using SalarySlip.API.Helpers;
using SalarySlip.API.Models.Domain;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace SalarySlip.API.Repositories
{
    public class UserlistRepository: IUserlistRepository
    {
        SqlConnection con;
        private readonly IConfiguration _configuration;
        public UserlistRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            con = new SqlConnection(_configuration.GetConnectionString("SalarySlip"));
        }

        public string Add(Userlist user)
        {
            //Generate user id
            /*int userId = 0;
            SqlDataAdapter da = new SqlDataAdapter("usp_GetUserMaxId",con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                userId = Convert.ToInt32(dt.Rows[0]["UserId"]);
            }
            
            userId++;*/
            //
            //User detail is inserted here
            SqlCommand cmd = new SqlCommand("usp_AddUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            string password = PasswordHasher.HashPassword(user.Password);
            //cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@UserName", user.UserName);
            cmd.Parameters.AddWithValue("@Password", password);
            
            cmd.Parameters.Add("@Result",SqlDbType.Bit);
            cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
            //int rowsAffected = 0; 
            string msg = "";
            try
            {
                con.Open();
                //int rowsAffected = cmd.ExecuteNonQuery();
                //Console.WriteLine("The rows affected vare : " + rowsAffected);
                cmd.ExecuteNonQuery();
                bool result = Convert.ToBoolean(cmd.Parameters["@Result"].Value);
                
                if (result)
                    msg = "Registration successful";
                else
                    msg = "User exists already!";
            }
            catch (Exception ex)
            {
                //Console.WriteLine("The exception msg is : " + ex.Message);
                msg = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return msg;
        }
    }
}
