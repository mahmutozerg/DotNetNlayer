using DotNetNlayer.Core.Configurations;
using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.DTO.Tokens;
using DotNetNlayer.Core.Models;

namespace DotNetNlayer.Core.Services;

public interface ITokenService
{
    Task<TokenDto> CreateTokenAsync(AppUser appUser);
    ClientTokenDto CreateTokenByClient(Client client);

}