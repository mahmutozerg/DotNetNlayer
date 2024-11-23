using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class AppUserRemoveFromRoleDto
{
    
    [Required(ErrorMessage = $"{nameof(UserId)} is Required")] public string UserId { get; set; }
    
    [Required(ErrorMessage = $"{nameof(RoleName)} is Required")] public string RoleName { get; set; }
}