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
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;
     public UserController(IUserService userService, IGenericService<AppUser> genericService, IAuthenticationService authenticationService)
     {
         _userService = userService;
         _authenticationService = authenticationService;
     }

    [HttpPost]
    public async Task<IActionResult> CreateUser(AppUserCreateDto createAppUserDto)
    {
            
            
        var result = await _userService.CreateAsync(createAppUserDto);
        
        if (result.StatusCode != 200) 
            return new ObjectResult(result);
        
        var loginD = new AppUserLoginDto()
        {
            Email = createAppUserDto.Email,
            Password = createAppUserDto.Password
        };
        var token = await _authenticationService.CreateTokenAsync(loginD);
        return  new ObjectResult(token);

    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        return  new ObjectResult(await _userService.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {

        return  new ObjectResult(await _userService.RemoveAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
    }
    
    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateUser(AppUserUpdateDto userUpdateDto)
    {

        return  new ObjectResult(await _userService.UpdateAsync(userUpdateDto,(ClaimsIdentity)User.Identity));
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateUserPassword(AppUserUpdatePasswordDto userUpdateDto)
    {

        return  new ObjectResult(await _userService.UpdatePasswordAsync(userUpdateDto,(ClaimsIdentity)User.Identity));
    }

}