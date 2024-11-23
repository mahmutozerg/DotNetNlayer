using DotNetNlayer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNlayer.API.Controllers.AdminControllers;

[ApiController]
[Route("/[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class AdminRoleController :ControllerBase
{
    
    private readonly IAppAuthenticationService _appAuthenticationService;

    public AdminRoleController(IAppAuthenticationService appAuthenticationService)
    {
        _appAuthenticationService = appAuthenticationService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddRole(string role)
    {
        var result = await _appAuthenticationService.AddRoleAsync(role);

        return new ObjectResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetAllRoles()
    {
        return new ObjectResult(await _appAuthenticationService.GetAllRolesAsync());
    }

}