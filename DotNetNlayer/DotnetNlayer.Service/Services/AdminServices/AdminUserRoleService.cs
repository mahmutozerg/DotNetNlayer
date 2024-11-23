using System.Net;
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
    public AdminUserRoleService( IUnitOfWork unitOfWork, RoleManager<AppRole> roleManager, IAdminUserRoleRepository userRepository, UserManager<AppUser> userManager) : base(userRepository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _userRepository = userRepository;
        _userManager = userManager;
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
        var validRoles = new HashSet<string>();
        var invalidRoles = new HashSet<string>();

        if (user == null)
        {
            return CustomResponseDto<NoDataDto>.
                Fail(ResponseMessages.NotFound,
                    (int)HttpStatusCode.NotFound,
                    $"{identifier} not found in records");
        }

        foreach (var roleName in roleNames)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
                validRoles.Add(roleName); 
            else
                invalidRoles.Add(roleName); 
            
        }

        if (invalidRoles.Any())
        {
            return CustomResponseDto<NoDataDto>.
                Fail(ResponseMessages.NotFound,
                    (int)HttpStatusCode.NotFound,
                    $"The following roles were not found: {string.Join(", ", invalidRoles)} none of the roles added try with corrected roles");
        }

        if (validRoles.Count == 0)
        {
            return CustomResponseDto<NoDataDto>.
                Fail(ResponseMessages.BadRequest,
                    (int)HttpStatusCode.BadRequest,
                    "No valid roles were provided to add.");
        }

        var result = await _userManager.AddToRolesAsync(user, validRoles);

        if (result.Succeeded)
        {
            return CustomResponseDto<NoDataDto>.Success((int)HttpStatusCode.Created);
        }
       
        return CustomResponseDto<NoDataDto>.
            Fail(ResponseMessages.InternalServerError,
                (int)HttpStatusCode.InternalServerError,
                string.Join("",result.Errors.Select(e => e.Description)));
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