using DotNetNlayer.Core.DTO.Tokens;
using DotNetNlayer.Core.DTO.User;
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
        return new ObjectResult(await _adminUserRoleService.GetUsersInRolesAsync(roleName));
    }

    [HttpPost]
    public async Task<IActionResult> AddUserToRoles(AddUserToRolesDto addUserToRolesDto)
    {
        return new ObjectResult(await _adminUserRoleService.AddUserToRolesAsync(addUserToRolesDto.Roles, addUserToRolesDto.UserIdentifier));
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteUserFromRole(AppUserRemoveFromRoleDto dto)
    {
        return new ObjectResult(await _adminUserRoleService.RemoveAppUserFromRole(dto));
    }
}