using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class AppUserLoginDto
{
    [EmailAddress]
    [Required(ErrorMessage = $"{nameof(Email)} field is required")]
    public string Email { get; set; } = "test@test.com";

    [Required(ErrorMessage = $"{nameof(Password)} field is required")]
    public string Password { get; set; } = string.Empty;
}