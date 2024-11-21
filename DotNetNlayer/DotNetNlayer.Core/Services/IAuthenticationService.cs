using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.DTO.User;
using DotNetNlayer.Core.Models;
using SharedLibrary.DTO.Result;
using SharedLibrary.DTO.Tokens;

namespace DotNetNlayer.Core.Services;

public interface IAuthenticationService
{
    Task<CustomResponseDto<TokenDto>> CreateTokenAsync(UserLoginDto loginDto);

    Task<CustomResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

    Task<CustomResponseDto<NoDataDto>> RevokeRefreshToken(string refreshToken);

    CustomResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    Task<CustomResponseDto<NoDataDto>> AddRole(string role);

    Task<UserRefreshToken> GetUserRefreshTokenByEmail(string userEmail);


}