using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.Tokens;

public class AddUserToRolesDto
{
    [Required(ErrorMessage = $"{nameof(Roles)} field is required")]
    public HashSet<string> Roles { get; set; } = new();
    [Required(ErrorMessage = $"{nameof(UserIdentifier)} field is required")]

    public string UserIdentifier { get; set; } = string.Empty;
}