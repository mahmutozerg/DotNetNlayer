using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTO.Tokens;

public class CreateTokenByRefreshTokenDto
{
    [Required(ErrorMessage = "RefreshToken field is required")]
    public string RefreshToken { get; set; }
}