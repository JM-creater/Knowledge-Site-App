using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Service;
using KnowledgeSiteApp.Models.Dto;
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

        [HttpPost("add")]
        public async Task<IActionResult> RegisterUser([FromBody] Core.Dto.RegisterUserDto dto)
        {
            try
            {
                var user = await service.Register(dto);
                return Ok(user);
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
                
                return Ok(user);
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
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var user = await service.GetAllUser();

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("getAllAdmins")]
        public async Task<IActionResult> GetAdmins()
        {
            try
            {
                var user = await service.GetAllByAdmin();

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAdmin(int id)
        {
            try
            {
                var user = await service.GetById(id);

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

        [HttpPut("updateDetails/{id}")]
        public async Task<IActionResult> UpdateUserDetails(int id, [FromBody] UpdateUserDetailsDto dto)
        {
            try
            {
                var user = await service.UpdateDetails(id, dto);

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

        [HttpPut("activate/{id}")]
        public async Task<IActionResult> ActivationUser(int id)
        {
            try
            {
                var user = await service.ActivateUser(id);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivationUser(int id)
        {
            try
            {
                var user = await service.DeactivateUser(id);

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
