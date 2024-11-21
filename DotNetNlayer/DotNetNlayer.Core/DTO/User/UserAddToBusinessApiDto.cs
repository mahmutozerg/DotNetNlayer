using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class UserAddToBusinessApiDto
{
    [Required(ErrorMessage = "Id field is required")]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "MailAddress field is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string Email { get; set; } = string.Empty;
    
}