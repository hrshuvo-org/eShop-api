using System.ComponentModel.DataAnnotations;

namespace Framework.Core.Models.Dtos;

public class RegisterDto
{
    // [Required]
    // public string Username { get; set; }
    
    [Required]
    [StringLength(20, MinimumLength = 4)]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }
}