using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SalarySlip.API.Helpers;
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
         
            //cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.Add("@Password", SqlDbType.VarChar,250);
            cmd.Parameters["@Password"].Direction = ParameterDirection.Output;
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
                
                if (result)
                {
                    string password = cmd.Parameters["@Password"].Value.ToString();
                  
                    if (PasswordHasher.VerifyPassword(user.Password, password))
                    {
                        user.Token = CreateJwt(user);
                        Console.WriteLine(user.Token);
                        errorMsg = "Login Success";
                    }
                    else
                        errorMsg = "Please verify password";
                }
                else
                    errorMsg = "User not found";
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

        private string CreateJwt(Userlist user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role,user.UserRole),
                new Claim(ClaimTypes.Name,user.UserName)
            });
            var cretdentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                //Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = cretdentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
