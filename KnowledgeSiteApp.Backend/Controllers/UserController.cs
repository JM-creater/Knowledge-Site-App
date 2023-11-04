using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Core.Enum;
using KnowledgeSiteApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService service;
        public UserController(IUserService userService)
        {
            service = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterUserDto dto)
        {
            try
            {
                var user = await service.Register(dto);
                return Ok("Successfully Registered");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto dto)
        {
            try
            {
                var user = await service.Login(dto);
                
                return Ok("Successfully Login");
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var user = service.GetAllUser();

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("getAuthors")]
        public async Task<IActionResult> GetByAuthors()
        {
            try
            {
                var user = await service.GetByAuthor();

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update-password")]
        public async Task<IActionResult> Update(UpdatePasswordDto dto)
        {
            try
            {
                var user = await service.ResetPassword(dto);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}/profile-pic")]
        public async Task<IActionResult> UpdateProfilePic(int userId, string newProfilePicture)
        {
            try
            {
                var user = await service.UpdateProfilePic(userId, newProfilePicture);

                return Ok(user);    
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("validate")]
        public async Task<IActionResult> ValidateUser(string userName, ValidateUserDto dto)
        {
            try
            {
                var user = await service.Validate(userName, dto);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("updateDetails")]
        public async Task<IActionResult> UpdateUserDetails(string userName, [FromForm] UpdateUserDetailsDto dto)
        {
            try
            {
                var user = await service.UpdateDetails(userName, dto);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            try
            {
                var user = await service.Delete(userName);

                return Ok("Sucessfully Deleted a Author");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
