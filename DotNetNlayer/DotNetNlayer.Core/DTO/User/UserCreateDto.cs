using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class UserCreateDto
{
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [Required(ErrorMessage = "Email field is required")]
    public string Email { get; set; } = "test@test.com";
    
    [Required(ErrorMessage = "Password field is required")]
    public string Password { get; set; } = string.Empty;
    


}