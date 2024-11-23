using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class AppUserLoginDto
{
    [Required(ErrorMessage = $"{nameof(EMailorUserName)} field is required")]
    public string EMailorUserName { get; set; } = "test@test.com";

    [Required(ErrorMessage = $"{nameof(Password)} field is required")]
    public string Password { get; set; } = string.Empty;
}