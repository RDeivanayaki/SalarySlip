using Microsoft.AspNetCore.Mvc;
using SalarySlip.API.Models.Domain;

namespace SalarySlip.API.Repositories
{
    public interface IMonthlistRepository
    {
        IEnumerable<Monthlist> GetAll();
    }
}
