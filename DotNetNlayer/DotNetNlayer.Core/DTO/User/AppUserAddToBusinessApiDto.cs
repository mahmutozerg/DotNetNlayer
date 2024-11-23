using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class AppUserAddToBusinessApiDto
{
    [Required(ErrorMessage = $"{nameof(Id)} field is required")]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = $"{nameof(Email)} field is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string Email { get; set; } = string.Empty;
    
    
    [Required(ErrorMessage = $"{nameof(UserName)} field is required")]
    public string UserName { get; set; } = string.Empty;
    
}