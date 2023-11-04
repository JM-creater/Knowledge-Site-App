using AutoMapper;
using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Core.Enum;
using KnowledgeSiteApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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

        public async Task<bool> GetExistingUser(RegisterUserDto dto)
            => await context.Users
                            .Where(u => u.Username == dto.Username || u.Email == dto.Email)
                            .AnyAsync();

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));  
            }
        }
        public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
            }
        }
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
                if (await GetExistingUser(dto))
                    throw new InvalidOperationException("Username or Email already exists");

                var bookImagePath = await booksImages(dto.BooksImages);
                var profileImagePath = await profileImages(dto.ProfileImage);

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(dto.Password, out passwordHash, out passwordSalt);

                var newUser = mapper.Map<User>(dto);
                newUser.PasswordHash = passwordHash;
                newUser.PasswordSalt = passwordSalt;
                newUser.BooksImages = bookImagePath;
                newUser.ProfileImage = profileImagePath;

                newUser.Role = (int)UserRole.Admin;

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

                var user = await context.Users
                                        .Where(u => u.Username == dto.Username &&
                                        u.Email == dto.Email)
                                        .FirstOrDefaultAsync();
                if (user == null)
                    throw new InvalidOperationException("Admin not found");

                if (!VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
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

        public async Task<IEnumerable<GetByAuthorDto>> GetByAuthor()
        {
            try
            {
                var user = await context.Users
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

                user.ProfileImage = newProfilePicture;
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

                byte[] newPasswordHash, newPasswordSalt;
                CreatePasswordHash(dto.Password, out newPasswordHash, out newPasswordSalt);

                existingUser.PasswordHash = newPasswordHash;
                existingUser.PasswordSalt = newPasswordSalt;

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

        public async Task<User> UpdateDetails(string userName, UpdateUserDetailsDto dto)
        {
            try
            {
                var user = await context.Users
                                        .Where(u => u.Username == userName)
                                        .FirstOrDefaultAsync();

                if (user == null)
                    throw new InvalidOperationException("User not found");

                var updateImage = await booksImages(dto.BooksImages);

                byte[] newPasswordHash, newPasswordSalt;
                CreatePasswordHash(dto.Password, out newPasswordHash, out newPasswordSalt);

                var updateUser = mapper.Map<User>(dto);
                updateUser.PasswordHash = newPasswordHash;
                updateUser.PasswordSalt = newPasswordSalt;
                updateUser.BooksImages = updateImage;

                context.Update(updateUser);
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

    }
}
