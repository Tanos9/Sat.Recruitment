using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Models.User;
using Sat.Recruitment.Api.Services.Interfaces;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsersControllerRefactor : ControllerBase
    {
        private IUserService _userService;

        public UsersControllerRefactor(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("/user")]
        public async Task<IActionResult> CreateUser(UserCreateRequest userCreateRequest)
        {
            var result = await _userService.CreateUser(userCreateRequest);
            return Ok(result);
        }

        [HttpGet]
        [Route("/user")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }
    }
}
