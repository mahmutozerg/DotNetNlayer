using System.Security.Claims;
using DotNetNlayer.Core.DTO.User;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNlayer.API.Controllers;

[Route("/[controller]/[action]")]
[ApiController]
public class UserController:ControllerBase
{
    private readonly IAppUserService _appUserService;
    private readonly IAppAuthenticationService _appAuthenticationService;
     public UserController(IAppUserService appUserService, IGenericService<AppUser> genericService, IAppAuthenticationService appAuthenticationService)
     {
         _appUserService = appUserService;
         _appAuthenticationService = appAuthenticationService;
     }

    [HttpPost]
    public async Task<IActionResult> CreateUser(AppUserCreateDto createAppUserDto)
    {
            
            
        var result = await _appUserService.CreateAsync(createAppUserDto);
        
        if (result.StatusCode != 200) 
            return new ObjectResult(result);
        
        var loginD = new AppUserLoginDto()
        {
            EMailorUserName = createAppUserDto.Email,
            Password = createAppUserDto.Password
        };
        var token = await _appAuthenticationService.CreateTokenAsync(loginD);
        return  new ObjectResult(token);

    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        return  new ObjectResult(await _appUserService.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {

        return  new ObjectResult(await _appUserService.RemoveAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
    }
    
    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateUser(AppUserUpdateDto userUpdateDto)
    {

        return  new ObjectResult(await _appUserService.UpdateAsync(userUpdateDto,(ClaimsIdentity)User.Identity));
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateUserPassword(AppUserUpdatePasswordDto userUpdateDto)
    {

        return  new ObjectResult(await _appUserService.UpdatePasswordAsync(userUpdateDto,(ClaimsIdentity)User.Identity));
    }

}