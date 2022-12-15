using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Models.User;
using Sat.Recruitment.Api.Services.Interfaces;

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
        [Route("/user/create")]
        public IActionResult CreateUser(UserCreateRequest userCreateRequest)
        {
            _userService.CreateUser(userCreateRequest);
            return Ok(new Result() { IsSuccess = true });
        }
    }
}
