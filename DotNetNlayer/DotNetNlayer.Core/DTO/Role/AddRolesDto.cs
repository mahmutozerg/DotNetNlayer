using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.Role;

public class AddRolesDto
{
    [Required(ErrorMessage = $"{nameof(Roles)} field is required")]
    public HashSet<AddRoleDto>Roles { get; set; }
}