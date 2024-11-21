using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.Role;

public class AddRoleDto
{
    [Required]
    public required string RoleName { get; set; }
}