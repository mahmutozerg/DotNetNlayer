using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.DTO.User;
using DotNetNlayer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTO.Result;

namespace DotNetNlayer.API.Controllers;
[Route("/[controller]/[action]")]
[ApiController]
public class AuthController:ControllerBase
{
    private readonly IAppAuthenticationService _appAuthenticationService;

    public AuthController(IAppAuthenticationService appAuthenticationService)
    {
        _appAuthenticationService = appAuthenticationService;
    }
    [HttpGet]
    [Authorize]
    public Task<IActionResult> GetUserRole()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
         
         
        return Task.FromResult<IActionResult>(new ObjectResult( CustomResponseDto<string?>.Success(role,(int)HttpStatusCode.OK)));
    }
    

    
    [HttpPost]
    public  IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var result =  _appAuthenticationService.CreateTokenByClient(clientLoginDto);
        return new ObjectResult(result);
    }



    [HttpPost]
    public async Task<IActionResult> CreateToken(AppUserLoginDto loginDto)
    {
        return new ObjectResult(await _appAuthenticationService.CreateTokenAsync(loginDto));
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RevokeRefreshToken(string refreshToken)
    {
        return new ObjectResult(await _appAuthenticationService.RevokeRefreshTokenAsync(refreshToken));

    }

    [HttpPost]
    public async Task<IActionResult> CreateTokenByRefreshToken(string refreshToken)
    {
        return new ObjectResult(await _appAuthenticationService.CreateTokenByRefreshTokenAsync(refreshToken));
    }
    

    

}