using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.Client;

public class ClientLoginDto
{
    [Required(ErrorMessage = $"{nameof(Id)} field is required")]
    public string Id { get; set; }
    
    [Required(ErrorMessage = $"{nameof(Secret)} field is required")]
    public string Secret { get; set; }
}