using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Repositories.AdminRepositories;
using DotNetNlayer.Core.Services.AdminServices;
using Microsoft.AspNetCore.Identity;
using SharedLibrary;

namespace DotnetNlayer.Service.Services.AdminServices;

public class AdminUserService:GenericService<AppUser>,IAdminUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAdminUserRepository _adminUserRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AdminUserService(UserManager<AppUser> userManager, IAdminUserRepository adminUserRepository, IUnitOfWork unitOfWork):base(adminUserRepository,unitOfWork)
    {
        _userManager = userManager;
        _adminUserRepository = adminUserRepository;
        _unitOfWork = unitOfWork;
    }
}