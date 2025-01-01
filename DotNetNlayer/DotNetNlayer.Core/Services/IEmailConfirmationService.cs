using SharedLibrary.DTO.Result;

namespace DotNetNlayer.Core.Services;

public interface IEmailConfirmationService
{
    Task<CustomResponseDto<NoDataDto>> ConfirmEmailAsync(string email, string token);
    Task<string> GenerateEmailConfirmationTokenAsync(string email);
}