using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.User;

public class UserDeleteDto
{
    [Required(ErrorMessage = "Id is required")]
    public string Id { get; set; } 
}