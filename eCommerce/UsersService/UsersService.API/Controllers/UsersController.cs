using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.DTO;
using UserService.Core.ServiceContracts;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }


        //GET /api/Users/{userID}
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetUserByUserID(Guid userID)
        {
            if (userID == Guid.Empty)
            {
                return BadRequest("Invalid User ID");
            }

            UserDTO? response = await _usersService.GetUserByUserID(userID);

            if (response == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
