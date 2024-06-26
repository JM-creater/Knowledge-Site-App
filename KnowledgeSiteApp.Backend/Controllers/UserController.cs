﻿using KnowledgeSiteApp.Backend.Attributes;
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
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
        {
            var user = await service.Register(dto);

            return Ok(user);
        }

        [HttpPost("SaveAdminImage/{id}")]
        public async Task<IActionResult> SaveAdminImage(int id, [FromForm] CreateAdminImageDto dto)
        {
            var adminImage = await service.SaveAdminImage(id, dto);

            return Ok(adminImage);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto dto)
        {
            var user = await service.Login(dto);
                
            return Ok(user);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await service.ForgotPassword(dto);

            if(user == null)
            {
                return NotFound("Email not registered.");
            }

            return Ok(user);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await service.ResetPassword(dto);

            if (user != null)
            {
                return Ok(user);
            }

            return BadRequest("Failed to reset password");
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers(string search)
        {
            var user = await service.SeachUser(search);

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await service.GetAllUser();

            return Ok(user);
        }

        [HttpGet("getAllAdmins")]
        public async Task<IActionResult> GetAdmins()
        {
            var user = await service.GetAllByAdmin();

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAdmin(int id)
        {
            var user = await service.GetById(id);

            return Ok(user);
        }

        [HttpPut("{userId}/profile-pic")]
        public async Task<IActionResult> UpdateProfilePic(int userId, string newProfilePicture)
        {
            var user = await service.UpdateProfilePic(userId, newProfilePicture);

            return Ok(user);   
        }

        [HttpPut("validate")]
        public async Task<IActionResult> ValidateUser(string userName, ValidateUserDto dto)
        {
            var user = await service.Validate(userName, dto);

            return Ok(user);
        }

        [HttpPut("updateDetails/{id}")]
        public async Task<IActionResult> UpdateUserDetails(int id, [FromBody] UpdateUserDetailsDto dto)
        {
            var user = await service.UpdateDetails(id, dto);

            return Ok(user);
        }

        [HttpPut("activate/{id}")]
        public async Task<IActionResult> ActivationUser(int id)
        {
            var user = await service.ActivateUser(id);

            return Ok(user);
        }

        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivationUser(int id)
        {
            var user = await service.DeactivateUser(id);

            return Ok(user);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var user = await service.Delete(userName);

            return Ok("Sucessfully Deleted a Author");
        }
    }
}
