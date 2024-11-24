using DotNetNlayer.Core.DTO.Role;
using DotNetNlayer.Core.Models;
using SharedLibrary.DTO.Result;

namespace DotNetNlayer.Core.Services.AdminServices;

public interface IAdminRoleService:IGenericService<AppRole>
{
    Task<CustomResponseDto<NoDataDto>> AddRoleAsync(string role);
    Task<CustomResponseDto<NoDataDto>> AddRolesAsync(AddRolesDto roles);
    Task<CustomResponseDto<NoDataDto>> DeleteRoleAsync(string roleName);

}