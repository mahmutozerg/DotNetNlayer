using DotNetNlayer.Core.DTO.User;
using DotNetNlayer.Core.Models;
using SharedLibrary.DTO.Result;

namespace DotNetNlayer.Core.Services.AdminServices;

public interface IAdminUserRoleService:IGenericService<AppUser>
{
    
    Task<CustomResponseDto<IList<AppUser>>> GetUsersInRolesAsync(string roleName);

    Task<CustomResponseDto<NoDataDto>> AddUserToRolesAsync(HashSet<string> roleNames, string identifier);

    Task<CustomResponseDto<NoDataDto>> RemoveAppUserFromRole(AppUserRemoveFromRoleDto dto);
    
    
}