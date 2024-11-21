using DotNetNlayer.Core.DTO.User;
using DotNetNlayer.Core.Models;
using SharedLibrary.DTO.Result;

namespace DotNetNlayer.Core.Services;

public interface IUserService:IGenericService<AppUser>
{
    Task<CustomResponseDto<AppUser>> CreateUserAsync(UserCreateDto createUserDto);
    Task<CustomResponseDto<AppUser>> GetUserByNameAsync(string userName);
    Task<CustomResponseDto<AppUser>> GetUserByEmailAsync(string eMail);
    Task<CustomResponseDto<NoDataDto>> Remove(string id);
    Task<CustomResponseDto<NoDataDto>> AddRoleToUser(string userEmail, string roleName);

    Task<CustomResponseDto<List<AppUser>>> GetAllUsersByPage(string page);
    Task SendDeleteReqToBusinessApi(AppUser appUser);



}

