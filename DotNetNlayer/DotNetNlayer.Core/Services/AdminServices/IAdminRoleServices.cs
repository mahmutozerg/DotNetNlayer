using DotNetNlayer.Core.DTO.Role;
using DotNetNlayer.Core.Models;
using SharedLibrary.DTO.Result;

namespace DotNetNlayer.Core.Services.AdminServices;

public interface IAdminRoleServices:IGenericService<AppRole>
{
    Task<CustomResponseDto<NoDataDto>> AddRoleAsync(string role);
    Task<CustomResponseDto<NoDataDto>> AddRolesAsync(HashSet<AddRoleDto> roles);
}