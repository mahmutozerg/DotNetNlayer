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
    private readonly IAdminRoleServices _adminRoleServices;

    public AdminRoleController(IAppAuthenticationService appAuthenticationService, IAdminRoleServices adminRoleServices)
    {
        _appAuthenticationService = appAuthenticationService;
        _adminRoleServices = adminRoleServices;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddRole(AddRoleDto addRoleDto)
    {
        var result = await _adminRoleServices.AddRoleAsync(addRoleDto.RoleName);

        return new ObjectResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddRoles(AddRolesDto addRoleDto)
    {
        var result = await _adminRoleServices.AddRolesAsync(addRoleDto.Roles);

        return new ObjectResult(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        return new ObjectResult(await _appAuthenticationService.GetAllRolesAsync());
    }


    [HttpDelete]
    public Task<IActionResult> DeleteRole(string roles)
    {
        throw   new NotImplementedException();
    }

}