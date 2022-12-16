using Sat.Recruitment.Api.Models.User;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.DataAccess.Interfaces
{
    public interface IUserJsonRepository : IGenericRepository<User>
    {
        Task<bool> UserExists(User user);
    }
}
