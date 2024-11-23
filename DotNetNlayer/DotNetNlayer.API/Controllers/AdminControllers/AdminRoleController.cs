using DotNetNlayer.Core.DTO.Role;
using DotNetNlayer.Core.Services;
using DotNetNlayer.Core.Services.AdminServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNlayer.API.Controllers.AdminControllers;

[ApiController]
[Route("/[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class AdminRoleController :ControllerBase
{
    
    private readonly IAppAuthenticationService _appAuthenticationService;
    private readonly IAdminRoleService _adminRoleService;

    public AdminRoleController(IAppAuthenticationService appAuthenticationService, IAdminRoleService adminRoleService)
    {
        _appAuthenticationService = appAuthenticationService;
        _adminRoleService = adminRoleService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddRole(AddRoleDto addRoleDto)
    {
        var result = await _adminRoleService.AddRoleAsync(addRoleDto.RoleName);

        return new ObjectResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddRoles(AddRolesDto addRoleDto)
    {
        var result = await _adminRoleService.AddRolesAsync(addRoleDto);

        return new ObjectResult(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        return new ObjectResult(await _appAuthenticationService.GetAllRolesAsync());
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        return new ObjectResult(await _adminRoleService.RemoveRoleFromUser(roleName));
    }

}