using System.Net;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Repositories.AdminRepositories;
using DotNetNlayer.Core.Services.AdminServices;
using Microsoft.AspNetCore.Identity;
using SharedLibrary;
using SharedLibrary.DTO.Exceptions;
using SharedLibrary.DTO.Result;

namespace DotnetNlayer.Service.Services.AdminServices;

public class AdminUserRoleService:GenericService<AppUser>,IAdminUserRoleService
{
    private readonly IAdminUserRoleRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<AppRole> _roleManager;
    public AdminUserRoleService( IUnitOfWork unitOfWork, RoleManager<AppRole> roleManager, IAdminUserRoleRepository userRepository) : base(userRepository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _userRepository = userRepository;
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
}