using AutoMapper;
using Azure.Core;
using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Core.Encryption;
using KnowledgeSiteApp.Backend.Core.Enum;
using KnowledgeSiteApp.Backend.Core.ImageDirectory;
using KnowledgeSiteApp.Backend.Core.Token;
using KnowledgeSiteApp.Models.Dto;
using KnowledgeSiteApp.Models.Entities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace KnowledgeSiteApp.Backend.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        public UserService(AppDbContext dbcontext, IMapper Imapper, IConfiguration _configuration)
        {
            context = dbcontext;
            mapper = Imapper;
            configuration = _configuration;
        }

        public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] 
            {
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                // Add other claims as needed
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

        public async Task<User> Register(RegisterUserDto dto)
        {
            try
            {
                var existingFullName = await context.Users
                                                     .Where(u => u.LastName == dto.LastName && u.FirstName == dto.FirstName)
                                                     .FirstOrDefaultAsync();

                if (existingFullName != null)
                    throw new InvalidOperationException("Admin with name already exists");

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


        public async Task<User> Login(LoginUserDto dto)
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
                    throw new AuthenticationException("Invalid password");

                return user;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<IEnumerable<User>> SeachUser(string search)
        {
            var user = await context.Users
                                    .Where(u => u.FirstName.Contains(search) ||
                                            u.LastName.Contains(search) ||
                                            u.Username.Contains(search))
                                    .ToListAsync();

            if (string.IsNullOrWhiteSpace(search))
            {
                return await context.Users.ToListAsync();
            }

            return user;
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

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailSettings = configuration.GetSection("EmailSettings");
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["Sender"]));
            mimeMessage.To.Add(MailboxAddress.Parse(email));
            mimeMessage.Subject = subject;

            mimeMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(emailSettings["MailServer"], int.Parse(emailSettings["MailPort"]), false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(emailSettings["Sender"], emailSettings["Password"]);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }

        public async Task SendPasswordResetEmail(string email, string token) 
        {
            string resetLink = $"http://127.0.0.1:5173/forgot_password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";
            string subject = "Password Reset Request";
            string message = $"Please click on the link to reset your password: <a href='{resetLink}'>Reset Password</a>";

            await SendEmailAsync(email, subject, message);
        }

        public async Task<User> ForgotPassword(string email)
        {
            try
            {
                var user = await context.Users
                                        .Where(u => u.Email == email)
                                        .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }

                user.PasswordResetToken = RandomToken.CreateRandomToken();
                user.ResetTokenExpires = DateTime.Now.AddDays(1);

                context.Users.Update(user);
                await context.SaveChangesAsync();

                await SendPasswordResetEmail(user.Email, user.PasswordResetToken);

                return user;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<User> ResetPassword(ResetPasswordDto dto)
        {
            try
            {
                var user = await context.Users
                                        .Where(u => u.PasswordResetToken == dto.Token)
                                        .FirstOrDefaultAsync();

                if (user == null || user.ResetTokenExpires < DateTime.Now)
                {
                    throw new InvalidOperationException("Invalid Token.");
                }

                user.Password = PasswordHasher.EncryptPassword(dto.NewPassword);
                user.PasswordResetToken = null;
                user.ResetTokenExpires = null;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return user;
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

                mapper.Map(dto, user);
                user.Username = dto.Username;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Email = dto.Email;

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

        public async Task<User> SaveAdminImage(int id, CreateAdminImageDto dto)
        {
            var adminImage = await context.Users.FindAsync(id);

            if (adminImage == null)
                throw new InvalidOperationException("Profile not found");

            var adminImageUpload = await new ImagePathConfig().profileImages(dto.Image);

            adminImage.Image = adminImageUpload;

            await context.SaveChangesAsync();

            return adminImage;
        }

    }
}
