using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.Role;

public class AddRoleDto
{
    [Required(ErrorMessage = $"{nameof(RoleName)} field is required")]
    public required string RoleName { get; set; }
}