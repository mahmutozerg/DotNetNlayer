using System.Security.Claims;
using DotNetNlayer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNlayer.API.Controllers.AdminControllers;

[ApiController]
[Route("/[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class AdminUserController:ControllerBase
{
    private readonly IAppUserService _appUserService;

    public AdminUserController(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser(string id)
    {
        return  new ObjectResult(await _appUserService.RemoveAsync(id));
    }
    
    
    
}