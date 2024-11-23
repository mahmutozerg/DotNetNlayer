using DotNetNlayer.Core.Services;
using DotNetNlayer.Core.Services.AdminServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNlayer.API.Controllers.AdminControllers;

[ApiController]
[Route("/[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class AdminUserRoleController:ControllerBase
{
    private readonly IAdminUserRoleService  _adminUserRoleService;

    public AdminUserRoleController(IAdminUserRoleService adminUserRoleService)
    {
        _adminUserRoleService = adminUserRoleService;
    }


    [HttpGet]
    public async Task<IActionResult> GetUsersInRole(string roleName)
    {
        return new ObjectResult(await _adminUserRoleService.GetUsersWithRolesAsync(roleName));
    }
}