using AutoMapper;
using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Core.Enum;
using KnowledgeSiteApp.Models.Dto;
using KnowledgeSiteApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace KnowledgeSiteApp.Backend.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public UserService(AppDbContext dbcontext, IMapper Imapper)
        {
            context = dbcontext;
            mapper = Imapper;
        }

        public async Task<User> GetById(int id)
    => await context.Users
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();

        public async Task<bool> GetExistingUser(RegisterUserDto dto)
            => await context.Users
                            .Where(u => u.Username == dto.Username || u.Email == dto.Email)
                            .AnyAsync();

        public async Task<string?> booksImages(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            string mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            string subFolder = Path.Combine(mainFolder, "BooksImages");

            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
            }
            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(subFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Path.Combine("Images", "BooksImages", fileName);
        }

        public async Task<string?> profileImages(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            string mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            string subFolder = Path.Combine(mainFolder, "ProfileImage");

            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
            }
            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(subFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Path.Combine("Images", "ProfileImage", fileName);
        }

        public async Task<User> Register(RegisterUserDto dto)
        {
            try
            {
                //if (await GetExistingUser(dto))
                //    throw new InvalidOperationException("Username or Email already exists");

                //var profileImagePath = await profileImages(dto.ProfileImage);
                var newPassword = EncryptPassword(dto.Password);
                var newUser = mapper.Map<User>(dto);

                newUser.Password = newPassword;
                newUser.FirstName = dto.FirstName;
                newUser.LastName = dto.LastName;
                newUser.Email = dto.Email;
                //newUser.ProfileImage = profileImagePath;

                newUser.Role = (int)UserRole.Admin;
                newUser.IsActive = true;
                newUser.IsValidate = true;

                await context.AddAsync(newUser);
                await context.SaveChangesAsync();

                return mapper.Map<User>(newUser);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<User> Login(LoginUserDto dto)
        {
            try
            {

                User user = await context.Users
                                        .Where(u => u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail)
                                        .FirstOrDefaultAsync();
                if (user == null)
                {
                    user = await context.Users
                                        .Where(u => u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail)
                                        .FirstOrDefaultAsync();
                }

                if (user == null)
                    throw new InvalidOperationException("Admin not found");

                if (!VerifyPassword(dto.Password, EncryptPassword(dto.Password)))
                    throw new InvalidOperationException("Invalid password");

                return mapper.Map<User>(user);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<User> GetAllUser()
            => context.Users.AsEnumerable();

        public async Task<List<User>> GetAllByAdmin()
           => await context.Users
                           .Where(u => u.Role == (int)UserRole.Admin)
                           .ToListAsync();

                var getUser = mapper.Map<IEnumerable<GetByAuthorDto>>(user);

                return getUser;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<User> UpdateProfilePic(int userId, string newProfilePicture)
        {
            try
            {
                var user = await context.Users
                                        .Where(u => u.Id == userId)
                                        .FirstOrDefaultAsync();

                if (user == null)
                    throw new Exception("Admin not found");

                //user.ProfileImage = newProfilePicture;
                context.Users.Update(user);
                await context.SaveChangesAsync();

                return user;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<User> ResetPassword(UpdatePasswordDto dto)
        {
            try
            {
                var existingUser = await context.Users
                                                .Where(u => u.Username == dto.Username)
                                                .FirstOrDefaultAsync();

                if (existingUser == null)
                    throw new InvalidOperationException("Admin not found");

                var newPassword = EncryptPassword(dto.Password);

                existingUser.Password = newPassword;

                await context.SaveChangesAsync();

                return mapper.Map<User>(existingUser);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<User> Validate(string userName, ValidateUserDto dto)
        {
            try
            {
                var userExist = await context.Users
                                             .Where(u => u.Username == userName)
                                             .FirstOrDefaultAsync();    

                if (userExist == null)
                    throw new InvalidOperationException("Admin not found");

                if (userExist.Role != (int)UserRole.Admin)
                    throw new Exception("The provided username does not correspont to a admin");

                mapper.Map<User>(userExist);
                userExist.IsActive = dto.IsActive;
                userExist.IsValidate = dto.IsValidate;

                await context.SaveChangesAsync();

                return userExist;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<User> UpdateDetails(int id, UpdateUserDetailsDto dto)
        {
            try
            {
                var user = await context.Users
                                        .Where(u => u.Id == id)
                                        .FirstOrDefaultAsync();

                if (user == null)
                    throw new InvalidOperationException("User not found");

                var encryptedPassword = EncryptPassword(dto.Password);

                mapper.Map(dto, user);
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Email = dto.Email;
                user.Password = encryptedPassword;

                user.Role = (int)UserRole.Admin;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<User> Delete(string userName)
        {
            try
            {
                var user = await context.Users
                                        .Where(u => u.Username == userName)
                                        .FirstOrDefaultAsync();

                if (user == null)
                    throw new InvalidOperationException("User not found");

                context.Remove(user);

                await context.SaveChangesAsync();

                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<User> ActivateUser(int id)
        {
            try
            {
                var user = await context.Users
                                        .Where(u => u.Id == id)
                                        .FirstOrDefaultAsync();
                if (user == null)
                    throw new InvalidOperationException("User not found");

                user.IsActive = true;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return user;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<User> DeactivateUser(int id)
        {
            try
            {
                var user = await context.Users
                                        .Where(u => u.Id == id)
                                        .FirstOrDefaultAsync();
                if (user == null)
                    throw new InvalidOperationException("User not found");

                user.IsActive = false;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return user;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public static string EncryptPassword(string password)
        {
            var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var enteredHash = EncryptPassword(enteredPassword);
            return string.Equals(enteredHash, storedHash, StringComparison.Ordinal);
        }

    }
}
