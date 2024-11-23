using System.Linq.Expressions;
using System.Net;
using DotNetNlayer.Core.DTO.Role;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories.AdminRepositories;
using DotNetNlayer.Core.Services.AdminServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SharedLibrary;
using SharedLibrary.Constants.Response;
using SharedLibrary.DTO.Exceptions;
using SharedLibrary.DTO.Result;

namespace DotnetNlayer.Service.Services.AdminServices;

public class AdminRoleService:GenericService<AppRole>,IAdminRoleService
{
    
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IAdminRoleRepository _adminRoleRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public AdminRoleService(RoleManager<AppRole> roleManager, IAdminRoleRepository adminRoleRepository, IUnitOfWork unitOfWork) : base(adminRoleRepository,unitOfWork)
    {
        _roleManager = roleManager;
        _adminRoleRepository = adminRoleRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<CustomResponseDto<NoDataDto>> AddRoleAsync(string role)
    {
        var roleEntity = await _roleManager.FindByNameAsync(role);
        if (roleEntity is not null)
            throw new AlreadyExistException(nameof(role), role);
        
        var result = await _roleManager.CreateAsync(new AppRole(role));
        
        if (!result.Succeeded)
            return CustomResponseDto<NoDataDto>.
                Fail(ResponseMessages.InternalServerError,
                    (int)HttpStatusCode.InternalServerError,
                    string.Join("",result.Errors.Select(e => e.Description)));
        
        
        return CustomResponseDto<NoDataDto>.Success(StatusCodes.Status200OK);

    }




    public async Task<CustomResponseDto<NoDataDto>> AddRolesAsync(AddRolesDto roles)
    {


        foreach (var role in roles.RoleNames)
        {
            var result = await AddRoleAsync(role);
            if(!result.IsSuccess)
                return result;
                
        }
        return CustomResponseDto<NoDataDto>.Success((int)HttpStatusCode.Created);
    }



    /// <summary>
    ///  returns Fail if any of the roles exist
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    
    public async Task<CustomResponseDto<NoDataDto>> CheckIfRoleExistResultFactoryAsync(HashSet<string> roles)
    {
        var validRoles = new HashSet<string>();
        var invalidRoles = new HashSet<string>();
        
        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
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
        return CustomResponseDto<NoDataDto>.Success((int)HttpStatusCode.OK);
    }
}