using AutoMapper;
using Azure.Core;
using KnowledgeSiteApp.Backend.Authentication;
using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Core.Encryption;
using KnowledgeSiteApp.Backend.Core.Enum;
using KnowledgeSiteApp.Backend.Core.ImageDirectory;
using KnowledgeSiteApp.Models.Dto;
using KnowledgeSiteApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace KnowledgeSiteApp.Backend.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public UserService(AppDbContext dbcontext, IMapper Imapper, UserManager<ApplicationUser> _userManager, IConfiguration _configuration)
        {
            context = dbcontext;
            mapper = Imapper;
            userManager = _userManager;
            configuration = _configuration;
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        private string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }

        public async Task<User> Register(Core.Dto.RegisterUserDto dto)
        {
            try
            {
                var userByEmail = await userManager.FindByEmailAsync(dto.Email);
                var userByUsername = await userManager.FindByNameAsync(dto.Username);

                if (userByEmail != null || userByUsername != null)
                {
                    throw new InvalidOperationException("Admin with the given email or username already exists.");
                }

                ApplicationUser applicationUser = new ApplicationUser
                {
                    Email = dto.Email,
                    UserName = dto.Username,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var identityResult = await userManager.CreateAsync(applicationUser, dto.Password);
                if (!identityResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to register ApplicationUser: {GetErrorsText(identityResult.Errors)}");
                }

                var existingFirstname = await context.Users
                                                .Where(u => u.FirstName == dto.FirstName)
                                                .FirstOrDefaultAsync();

                if (existingFirstname != null)
                    throw new InvalidOperationException("Admin with first name already exists");

                var existingLastname = await context.Users
                                                     .Where(u => u.LastName == dto.LastName)
                                                     .FirstOrDefaultAsync();

                if (existingLastname != null)
                    throw new InvalidOperationException("Admin with last name already exists");

                User newUser = mapper.Map<User>(dto);
                newUser.Password = PasswordHasher.EncryptPassword(dto.Password);
                newUser.Role = (int)UserRole.Admin;
                newUser.IsActive = true;
                newUser.DateCreated = DateTime.Now;

                context.Users.Add(newUser);
                await context.SaveChangesAsync();

                return mapper.Map<User>(newUser);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }


        public async Task<string> Login(LoginUserDto dto)
        {
            try
            {
                User user = await context.Users
                                         .Where(u => u.Username == dto.UsernameOrEmail
                                                  || u.Email == dto.UsernameOrEmail)
                                         .FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(dto.UsernameOrEmail))
                    throw new AuthenticationException("Either Email or Username must be provided.");

                if (user == null)
                    throw new InvalidOperationException("User not found");

                if (!PasswordHasher.VerifyPassword(dto.Password, user.Password))
                    throw new InvalidOperationException("Invalid password");

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var token = GetToken(authClaims);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<User> GetById(int id)
           => await context.Users
                           .Where(u => u.Id == id)
                           .FirstOrDefaultAsync();

        public async Task<List<User>> GetAllUser()
           => await context.Users
                           .ToListAsync();

        public async Task<List<User>> GetAllByAdmin()
           => await context.Users
                           .Where(u => u.Role == (int)UserRole.Admin)
                           .ToListAsync();

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
                throw new ArgumentException(e.Message);
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

                var newPassword = PasswordHasher.EncryptPassword(dto.Password);

                existingUser.Password = newPassword;

                await context.SaveChangesAsync();

                return mapper.Map<User>(existingUser);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
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

                var encryptedPassword = PasswordHasher.EncryptPassword(dto.Password);

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
                throw new ArgumentException(e.Message);
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

        public async Task<User> Delete(string userName)
        {
            try
            {
                var user = await context.Users
                                        .Where(u => u.Username == userName)
                                        .FirstOrDefaultAsync();

                if (user == null)
                    throw new InvalidOperationException("User not found");

                context.Users.Remove(user);

                await context.SaveChangesAsync();

                return user;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

    }
}
