using DotNetNlayer.Core.DTO.Role;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Repositories.AdminRepositories;
using DotNetNlayer.Core.Services.AdminServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SharedLibrary;
using SharedLibrary.DTO.Exceptions;
using SharedLibrary.DTO.Result;

namespace DotnetNlayer.Service.Services.AdminServices;

public class AdminRoleServiceIAdminRoleServices:GenericService<AppRole>,IAdminRoleServices
{
    
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IAdminRoleRepository _adminRoleRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public AdminRoleServiceIAdminRoleServices(RoleManager<AppRole> roleManager, IAdminRoleRepository adminRoleRepository, IUnitOfWork unitOfWork) : base(adminRoleRepository,unitOfWork)
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
        
        await _roleManager.CreateAsync(new AppRole(role));
        
        return CustomResponseDto<NoDataDto>.Success(StatusCodes.Status200OK);

    }

    public Task<CustomResponseDto<NoDataDto>> AddRolesAsync(HashSet<AddRoleDto> roles)
    {

        throw new NotImplementedException();
    }
}