using DotNetNlayer.Core.Models;
using SharedLibrary.DTO.Result;

namespace DotNetNlayer.Core.Services.AdminServices;

public interface IAdminUserRoleService:IGenericService<AppUser>
{
    
    Task<CustomResponseDto<IList<AppUser>>> GetUsersWithRolesAsync(string roleName);

    Task<CustomResponseDto<NoDataDto>> AddUserToRolesAsync(HashSet<string> roleNames, string identifier);

}