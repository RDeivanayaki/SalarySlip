using SalarySlip.API.Models.Domain;

namespace SalarySlip.API.Repositories
{
    public interface IUserLoginDetailRepository
    {
        string Login(Userlist user);
    }
}
