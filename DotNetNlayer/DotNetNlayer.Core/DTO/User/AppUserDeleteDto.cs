using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class AppUserDeleteDto
{
    [Required(ErrorMessage = $"{nameof(Id)} is required")]
    public string Id { get; set; } 
}