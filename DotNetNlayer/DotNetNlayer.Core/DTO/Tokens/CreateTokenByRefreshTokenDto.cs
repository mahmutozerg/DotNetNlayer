using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.DTO.Tokens;

public class CreateTokenByRefreshTokenDto
{
    [Required(ErrorMessage = "RefreshToken field is required")]
    public string RefreshToken { get; set; }
}