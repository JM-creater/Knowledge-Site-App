using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Dto;
using KnowledgeSiteApp.Models.Entities;
namespace KnowledgeSiteApp.Backend.Service
{
    public interface IUserService
    {
        public Task<User> Register(RegisterUserDto dto);
        public Task<User> Login(LoginUserDto dto);
        public IEnumerable<User> GetAllUser();
        public Task<IEnumerable<GetByAuthorDto>> GetByAuthor();
        public Task<User> ResetPassword(UpdatePasswordDto dto);
        public Task<User> UpdateProfilePic(int userId, string newProfilePicture);
        public Task<User> Validate(string userName, ValidateUserDto dto);
        public Task<User> UpdateDetails(int id, UpdateUserDetailsDto dto);
        public Task<User> Delete(string userName);
        public Task<User> ActivateUser(int id);
       public Task<User> DeactivateUser(int id);
        public Task<User> GetById(int id);
    }
}
