using DotNetNlayer.Core.Models;

namespace DotNetNlayer.Core.Repositories.AdminRepositories;

public interface IAdminUserRoleRepository:IGenericRepository<AppUser>
{
    Task<IList<AppUser>> GetUsersInRolesAsync(string roleName);
    
}