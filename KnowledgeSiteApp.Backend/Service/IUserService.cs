using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Dto;
using KnowledgeSiteApp.Models.Entities;
namespace KnowledgeSiteApp.Backend.Service
{
    public interface IUserService
    {
        public Task<User> Register(Core.Dto.RegisterUserDto dto);
        public Task<string> Login(LoginUserDto dto);
        public Task<List<User>> GetAllUser();
        public Task<List<User>> GetAllByAdmin();
        public Task<User> GetById(int id);
        public Task<User> ResetPassword(UpdatePasswordDto dto);
        public Task<User> UpdateProfilePic(int userId, string newProfilePicture);
        public Task<User> Validate(string userName, ValidateUserDto dto);
        public Task<User> UpdateDetails(int id, UpdateUserDetailsDto dto);
        public Task<User> Delete(string userName);
        public Task<User> ActivateUser(int id);
        public Task<User> DeactivateUser(int id);
    }
}
