using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class UserLoginDto
{
    [EmailAddress]
    [Required(ErrorMessage = "Email field is required")]
    public string Email { get; set; } = "test@test.com";

    [Required(ErrorMessage = "Password field is required")]
    public string Password { get; set; } = string.Empty;
}