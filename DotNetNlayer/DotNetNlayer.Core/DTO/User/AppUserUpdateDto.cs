using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class AppUserUpdateDto
{
    [Required(ErrorMessage = $"{nameof(Id)} field is required")]
    public string Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;


}