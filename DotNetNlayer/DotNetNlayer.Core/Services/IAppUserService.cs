using System.Security.Claims;
using DotNetNlayer.Core.DTO.Tokens;
using DotNetNlayer.Core.DTO.User;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories;
using SharedLibrary.DTO.Result;

namespace DotNetNlayer.Core.Services;

public interface IAppUserService:IGenericService<AppUser>
{
    Task<CustomResponseDto<AppUser>> CreateAsync(AppUserCreateDto createAppUserDto);
    
    Task<CustomResponseDto<AppUser>> GetByIdAsync(string userId);
    Task<CustomResponseDto<AppUser>> GetByNameAsync(string userName);
    Task<CustomResponseDto<AppUser>> GetByEmailAsync(string eMail);
    Task<CustomResponseDto<NoDataDto>> RemoveAsync(string id);
    
    Task<CustomResponseDto<NoDataDto>> UpdateAsync(AppUserUpdateDto userUpdateDto, ClaimsIdentity userIdentity);

    

    Task<CustomResponseDto<TokenDto>> UpdatePasswordAsync(AppUserUpdatePasswordDto userUpdatePasswordDto,
        ClaimsIdentity claimsIdentity);



}

