using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Models.User;
using Sat.Recruitment.Api.Services.Interfaces;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates a new User
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("/users")]
        public async Task<IActionResult> CreateUser(UserCreateRequest userCreateRequest)
        {
            var result = await _userService.CreateUser(userCreateRequest);
            return CreatedAtAction(nameof(CreateUser), result);
        }

        /// <summary>
        /// Gets all Users
        /// </summary>
        [HttpGet]
        [Route("/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }
    }
}
