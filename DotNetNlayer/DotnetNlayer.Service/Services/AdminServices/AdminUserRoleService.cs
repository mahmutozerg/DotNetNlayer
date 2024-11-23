using System.Net;
using DotNetNlayer.Core.DTO.User;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Repositories.AdminRepositories;
using DotNetNlayer.Core.Services.AdminServices;
using DotNetNlayer.Core.Utils;
using Microsoft.AspNetCore.Identity;
using SharedLibrary;
using SharedLibrary.Constants.Response;
using SharedLibrary.DTO.Exceptions;
using SharedLibrary.DTO.Result;

namespace DotnetNlayer.Service.Services.AdminServices;

public class AdminUserRoleService:GenericService<AppUser>,IAdminUserRoleService
{
    private readonly IAdminUserRoleRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IAdminRoleService _adminRoleService;
    public AdminUserRoleService( IUnitOfWork unitOfWork, RoleManager<AppRole> roleManager, IAdminUserRoleRepository userRepository, UserManager<AppUser> userManager, IAdminRoleService adminRoleService) : base(userRepository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _userRepository = userRepository;
        _userManager = userManager;
        _adminRoleService = adminRoleService;
    }

    
    
    public async Task<CustomResponseDto<IList<AppUser>>> GetUsersWithRolesAsync(string roleName)
    {
        if(string.IsNullOrEmpty(roleName) || string.IsNullOrWhiteSpace(roleName))
            throw new ArgumentNullException(nameof(roleName), $"{nameof(roleName)}  is null or empty");

        var doesRoleExist = await _roleManager.RoleExistsAsync(roleName);
        if (!doesRoleExist)
            throw new NotFoundException(nameof(roleName), roleName);
        
        
        return CustomResponseDto<IList<AppUser>>.Success(await _userRepository.GetUsersWithRolesAsync(roleName),(int)HttpStatusCode.OK);
    }

    public async Task<CustomResponseDto<NoDataDto>> AddUserToRolesAsync(HashSet<string> roleNames, string identifier)
    {
        var user = await GetUserByIdentifier(identifier);


        if (user == null)
            throw new NotFoundException(nameof(identifier), identifier);
        
        
        var result = await _userManager.AddToRolesAsync(user, roleNames);

        if (result.Succeeded)
            return CustomResponseDto<NoDataDto>.Success((int)HttpStatusCode.Created);
        
       
        
        return CustomResponseDto<NoDataDto>.
            Fail(ResponseMessages.InternalServerError,
                (int)HttpStatusCode.InternalServerError,
                string.Join("",result.Errors.Select(e => e.Description)));
    }

    // The use of userId is by choice so that you don't accidently remove users. Id's seems less error prone even though are less readeble
    public async Task<CustomResponseDto<NoDataDto>> RemoveAppUserFromRole(AppUserRemoveFromRoleDto dto)
    {
        var role = await _roleManager.FindByNameAsync(dto.RoleName) ;
        
        if(role == null)
            throw new NotFoundException(nameof(dto.RoleName), dto.RoleName);
        
        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user == null)
            throw new NotFoundException(nameof(user), dto.UserId);

        var result = await _userManager.RemoveFromRoleAsync(user, role.Name!);
        
        if(!result.Succeeded)
            throw new SomethingWentWrongException(nameof(result),string.Join("",result.Errors.Select(e => e.Description)));
        
        return CustomResponseDto<NoDataDto>.Success((int)HttpStatusCode.OK);
    }

    private async Task<AppUser?> GetUserByIdentifier(string userIdentifier)
    {
        AppUser? user = null;

        if (ValidationUtils.IsValidGuid(userIdentifier))
        {
            user = await _userManager.FindByIdAsync(userIdentifier);
        }
        else if (ValidationUtils.IsValidEmail(userIdentifier))
        {
            user = await _userManager.FindByEmailAsync(userIdentifier);
        }
        else
        {
            user = await _userManager.FindByNameAsync(userIdentifier);
        }

        return user;
    }


}