using SalarySlip.API.Models.Domain;

namespace SalarySlip.API.Repositories
{
    public interface IUserlistRepository
    {
        string Add(Userlist user);
    }
}
