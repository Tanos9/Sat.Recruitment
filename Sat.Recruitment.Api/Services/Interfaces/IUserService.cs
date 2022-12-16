using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result> CreateUser(UserCreateRequest userCreateRequest);

        Task<IEnumerable<User>> GetAllUsers();
    }
}
