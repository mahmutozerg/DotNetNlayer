using DotNetNlayer.Core.Configurations;
using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.DTO.User;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary;
using SharedLibrary.DTO.Exceptions;
using SharedLibrary.DTO.Result;
using SharedLibrary.DTO.Tokens;

namespace DotnetNlayer.Service.Services;

public class AuthenticationService:IAuthenticationService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<UserRefreshToken> _refreshTokenService;
    private readonly List<ClientLoginDto >_tokenOptions;
    private readonly RoleManager<AppRole> _roleManager;


    public AuthenticationService(ITokenService tokenService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> refreshTokenService, IOptions<List<ClientLoginDto>> tokenOptions, RoleManager<AppRole> roleManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _refreshTokenService = refreshTokenService;
        _roleManager = roleManager;
        _tokenOptions = tokenOptions.Value;
    }

    
    public async Task<CustomResponseDto<TokenDto>> CreateTokenAsync(AppUserLoginDto loginDto)
    {
        ArgumentNullException.ThrowIfNull(loginDto);
        
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            throw new UserNotFoundException(nameof(UserRefreshToken), loginDto.Email);
        

        var token = await _tokenService.CreateTokenAsync(user);

        var userRefreshToken = await _refreshTokenService
            .Where(r => r != null && r.UserId == user.Id)
            .SingleOrDefaultAsync();

        if (userRefreshToken is null)
        {
            await _refreshTokenService.AddAsync(new()
                { UserId = user.Id, Token = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
        }
        else
        {
            userRefreshToken.Token = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
        }

        await _unitOfWork.SaveChangesAsync();
        return CustomResponseDto<TokenDto>.Success(token,200);
    }

    public async Task<CustomResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
    {
        
        var _refreshToken = await _refreshTokenService
            .Where(r => r != null && r.Token == refreshToken)
            .SingleOrDefaultAsync();

        if (_refreshToken is null)
            throw new NotFoundException(nameof(UserRefreshToken), refreshToken);
        
        var user = await _userManager.FindByIdAsync(_refreshToken.UserId);
        
        if (user is null)
            throw new NotFoundException(nameof(UserRefreshToken), _refreshToken.UserId);
        
        var token =await _tokenService.CreateTokenAsync(user);
        _refreshToken.Token = token.RefreshToken;
        _refreshToken.Expiration = token.RefreshTokenExpiration;

        await _unitOfWork.SaveChangesAsync();

        return CustomResponseDto<TokenDto>.Success(token,StatusCodes.Status200OK);
        
    }
    public CustomResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        if (_tokenOptions == null || !_tokenOptions.Any())
            throw new NotFoundException(nameof(_tokenOptions), "TokenOptions");
        
        
        foreach (var configuredClient in _tokenOptions)
        {
            if (string.CompareOrdinal(configuredClient.Id, clientLoginDto.Id) != 0 || string.CompareOrdinal(configuredClient.Secret, clientLoginDto.Secret) != 0) 
                continue;
            
            var client = new Client()
            {
                Id = clientLoginDto.Id,
                Secret = clientLoginDto.Secret,
                Audiences = ApiConstants.Aud
            };

            var clientTokenDto = _tokenService.CreateTokenByClient(client);
            return CustomResponseDto<ClientTokenDto>.Success(statusCode: 200, data: clientTokenDto);
        }

        throw new NotFoundException(nameof(_tokenOptions),string.Join("",_tokenOptions.Select(x=> x.Id).ToList()));
    }
    
    public async Task<CustomResponseDto<NoDataDto>> RevokeRefreshToken(string refreshToken)
    {
        var _refreshToken = await _refreshTokenService
            .Where(r => r != null && r.Token == refreshToken)
            .FirstOrDefaultAsync();

        if (refreshToken != null)
            throw new NotFoundException(nameof(_tokenOptions), refreshToken);

        _refreshTokenService.Remove(_refreshToken);
        await _unitOfWork.SaveChangesAsync();


        return CustomResponseDto<NoDataDto>.Success(StatusCodes.Status200OK);

    }

    public async Task<CustomResponseDto<NoDataDto>> AddRole(string role)
    {
        var roleEntity = await _roleManager.FindByNameAsync(role);
        if (roleEntity is not null)
            throw new AlreadyExistException(nameof(role), role);
        
        await _roleManager.CreateAsync(new AppRole(role));
        
        return CustomResponseDto<NoDataDto>.Success(StatusCodes.Status200OK);

    }

    public async Task<UserRefreshToken> GetUserRefreshTokenByEmail(string userEmail)
    {
        var user =await _userManager.FindByNameAsync(userEmail.Split("@")[0]);
        if (user is null)
            throw new NotFoundException("userEmail", userEmail);

        var refreshToken = await _refreshTokenService
            .Where(u => u.UserId == user.Id)
            .FirstOrDefaultAsync();

        if (refreshToken is null)
            throw new NotFoundException(nameof(UserRefreshToken), userEmail);

        return refreshToken;

    }
}