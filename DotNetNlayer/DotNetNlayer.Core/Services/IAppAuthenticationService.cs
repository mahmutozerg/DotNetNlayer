using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.DTO.Tokens;
using DotNetNlayer.Core.DTO.User;
using DotNetNlayer.Core.Models;
using SharedLibrary.DTO.Result;

namespace DotNetNlayer.Core.Services;

public interface IAppAuthenticationService
{
    Task<CustomResponseDto<TokenDto>> CreateTokenAsync(AppUserLoginDto loginDto);

    Task<CustomResponseDto<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken);

    Task<CustomResponseDto<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken);

    CustomResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    Task<CustomResponseDto<NoDataDto>> AddRoleAsync(string role);

    
    Task<CustomResponseDto<List<AppRole>>> GetAllRolesAsync();


}