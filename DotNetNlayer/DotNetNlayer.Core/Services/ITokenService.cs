using DotNetNlayer.Core.Configurations;
using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.Models;
using SharedLibrary.DTO.Tokens;

namespace DotNetNlayer.Core.Services;

public interface ITokenService
{
    Task<TokenDto> CreateTokenAsync(AppUser appUser);
    ClientTokenDto CreateTokenByClient(Client client);

}