using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using DotNetNlayer.Core.Configurations;
using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.DTO.Tokens;

namespace DotnetNlayer.Service.Services;

public class TokenService:ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppTokenOptions _tokenOptions;

    public TokenService(UserManager<AppUser> userManager, IOptions<AppTokenOptions> tokenOptions)
    {
        _userManager = userManager;
        _tokenOptions = tokenOptions.Value;
    }
   
    
    public async Task<TokenDto> CreateTokenAsync(AppUser appUser)
    {
        var accesTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration);
        var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var jwtSecurityToken = new JwtSecurityToken(

            issuer: _tokenOptions.Issuer,
            expires: accesTokenExpiration,
            notBefore: DateTime.Now,
            signingCredentials: signingCredentials,
            claims: await GetClaims(appUser, _tokenOptions.Audience)
        );

        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.WriteToken(jwtSecurityToken);

        var tokendto = new TokenDto
        {
            AccessToken = token,
            AccessTokenExpiration = accesTokenExpiration,
            RefreshToken = CreateRefreshToken(),
            RefreshTokenExpiration = refreshTokenExpiration
        };

        return tokendto;
    }



    public ClientTokenDto CreateTokenByClient(Client client)
    {
        var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var jwtSecurityToken = new JwtSecurityToken(
            
            issuer: _tokenOptions.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            signingCredentials: signingCredentials,
            claims: GetClaimsByClient(client)
        );

        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.WriteToken(jwtSecurityToken);

        var tokendto = new ClientTokenDto
        {
            AccesToken = token,
            AccesTokenExpiration = accessTokenExpiration
        };

        return tokendto;    
    }
    
    private static string CreateRefreshToken()
    {
        var numberByte = new Byte[64];

        using var random = RandomNumberGenerator.Create();
        random.GetBytes(numberByte);

        return Convert.ToBase64String(numberByte);
    }

    private async Task<IEnumerable<Claim>> GetClaims(AppUser user , List<String> aud)
    {
        var uRole = await _userManager.GetRolesAsync(user);
        
        var userClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        userClaims.AddRange(aud.Select(c => new Claim(JwtRegisteredClaimNames.Aud, c)));
        userClaims.AddRange(uRole.Select(userRole => new Claim(ClaimTypes.Role, userRole)));


        return userClaims;
    }

    private static IEnumerable<Claim> GetClaimsByClient(Client client)
    {
        var claims = new List<Claim>();
        claims.AddRange(client.Audiences.Select(a=> new Claim(JwtRegisteredClaimNames.Aud,a)));

        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.NameId, client.Id.ToString()));
        
        return claims;
    }

}