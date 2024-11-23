using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class AppUserUpdatePasswordDto
{
    [Required(ErrorMessage = $"{nameof(OldPassword)} field is required")]
    public string OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = $"{nameof(NewPassword)} field is required")]
    public string NewPassword { get; set; } = string.Empty;

    [Required] 
    [Compare(nameof(NewPassword),ErrorMessage = $"{nameof(NewPassword)} must match with {nameof(NewPasswordConfirm)}")]
    public string NewPasswordConfirm { get; set; } = string.Empty;
}