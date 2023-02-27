namespace SalarySlip.API.Models.Domain
{
    public class Userlist
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        public string Token { get; set; }
    }
}
