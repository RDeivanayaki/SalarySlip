using SalarySlip.API.Models.Domain;

namespace SalarySlip.API.Repositories
{
    public interface IYearlistRepository
    {
        IEnumerable<Yearlist> GetAll();
    }
}
