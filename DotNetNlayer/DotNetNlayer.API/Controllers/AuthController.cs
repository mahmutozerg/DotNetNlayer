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
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    
    [HttpPost]
    public async Task<IActionResult> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var result =  _authenticationService.CreateTokenByClient(clientLoginDto);
        return new ObjectResult(result);
    }



    [HttpPost]
    public async Task<IActionResult> CreateToken(AppUserLoginDto loginDto)
    {
        var result = await _authenticationService.CreateTokenAsync(loginDto);
        return new ObjectResult(result);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RevokeRefreshToken(string refreshToken)
    {
        var result = await _authenticationService.RevokeRefreshToken(refreshToken);
        return new ObjectResult(result);

    }

    [HttpPost]
    public async Task<IActionResult> CreateTokenByRefreshToken(string refreshToken)
    {
        var result = await _authenticationService.CreateTokenByRefreshToken(refreshToken);
        return new ObjectResult(result);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddRole(string role)
    {
        var result = await _authenticationService.AddRole(role);

        return new ObjectResult(result);
    }
     
     
    [HttpGet]
    [Authorize]
    public Task<IActionResult> GetRole()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
         
         
        return Task.FromResult<IActionResult>(new ObjectResult( CustomResponseDto<string?>.Success(role,(int)HttpStatusCode.OK)));
    }
}