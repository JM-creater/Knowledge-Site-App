using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSiteApp.Models.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ProfileImage { get; set; }
    public string WebsiteUrl { get; set; }
    public string ArticlesUrl { get; set; }
    public string BooksImages { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public int Role { get; set; }
    public bool IsActive { get; set; } 
    public bool IsValidate { get; set; } 
}
