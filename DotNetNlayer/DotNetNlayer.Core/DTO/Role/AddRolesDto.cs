using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.Role;

public class AddRolesDto
{
    [Required(ErrorMessage = $"{nameof(RoleNames)} field is required")]
    public HashSet<string>RoleNames { get; set; }
}