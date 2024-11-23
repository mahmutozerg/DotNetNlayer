using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class AppUserCreateDto
{
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [Required(ErrorMessage = "Email field is required")]
    public string Email { get; set; } = "test@test.com";
    
    [Required(ErrorMessage = "Password field is required")]
    public string Password { get; set; } = string.Empty;
    [Required(ErrorMessage = "ConfirmPassword field is required")]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Username field is required")]
    public string UserName { get; set; } = string.Empty;


}