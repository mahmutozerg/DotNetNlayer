using System.Net;
using System.Text;
using Azure;
using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.DTO.User;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedLibrary;
using SharedLibrary.Constants.Response;
using SharedLibrary.DTO.Exceptions;
using SharedLibrary.DTO.Result;

namespace DotnetNlayer.Service.Services;

public class UserService : GenericService<AppUser>, IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IGenericRepository<AppUser> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IAuthenticationService _authenticationService;
    private readonly List<ClientLoginDto> _clientTokenOptions;

    public UserService(UserManager<AppUser> userManager, IGenericRepository<AppUser> repository, IUnitOfWork unitOfWork,
        RoleManager<AppRole> roleManager, IAuthenticationService authenticationService,
        IOptions<List<ClientLoginDto>> clientTokenOptions) : base(repository, unitOfWork)
    {
        _userManager = userManager;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _authenticationService = authenticationService;
        _clientTokenOptions = clientTokenOptions.Value;
    }

    public async Task<CustomResponseDto<AppUser>> CreateUserAsync(UserCreateDto createUserDto)
    {

        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(), 
            Email = createUserDto.Email,
            UserName = createUserDto.Email.Split("@")[0],
            CreatedAt = DateTime.Now, CreatedBy = "System"
        };
        var result = await _userManager.CreateAsync(user, createUserDto.Password);

        if (!result.Succeeded)
            throw new AlreadyExistException(nameof(user), string.Join("",result.Errors.Select(x => x.Description).ToList()));

        /*
         * This is for creating the same user in business database
         * when a new user created we are sending the request to the business api
         * For that we need a bearer token(client token) which we are going to get from authentication service
         * User/AddById endpoint is authorized with a policy according to that so only a authserver client can reach there
         */

        await SendReqToBusinessApiAddById(user);


        return CustomResponseDto<AppUser>.Success(user, 200);
    }


    private async Task SendReqToBusinessApiAddById(AppUser user)
    {
        using var client = new HttpClient();
        
        const string url = ApiConstants.BusinessApiIp + "/api/User/AddById";

        var requestData = new UserAddToBusinessApiDto()
        {
            Id = user.Id,
            Email = user.Email,

        };

        var jsonData = JsonConvert.SerializeObject(requestData);

        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var clientToken = _authenticationService.CreateTokenByClient(
            new ClientLoginDto()
            {
                Id = _clientTokenOptions[0].Id,
                Secret = _clientTokenOptions[0].Secret
            }
        );
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken.Data.AccesToken);
        
        var response = await client.PostAsync(url, content);
        
        if (response.StatusCode != HttpStatusCode.Created)
            throw new OutOfReachException(url);
    }

    public async Task<CustomResponseDto<AppUser>> GetUserByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return user is null ? 
            throw new UserNotFoundException(nameof(user),userName) 
            : CustomResponseDto<AppUser>.Success(user,StatusCodes.Status200OK);
    }

    public async Task<CustomResponseDto<AppUser>> GetUserByEmailAsync(string eMail)
    {
        var user = await _userManager.FindByEmailAsync(eMail);
        return user is null ? 
            throw new UserNotFoundException(nameof(user), eMail)
            : CustomResponseDto<AppUser>.Success(user, 200);
    }

    public async Task<CustomResponseDto<NoDataDto>> Remove(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            throw new UserNotFoundException(nameof(user), id);

        _repository.Remove(user);
        await _unitOfWork.SaveChangesAsync();

        await SendDeleteReqToBusinessApi(user);
        return CustomResponseDto<NoDataDto>.Success(200);
    }

    public async Task<CustomResponseDto<NoDataDto>> AddRoleToUser(string userEmail, string roleName)
    {
        var user = await _userManager.FindByNameAsync(userEmail.Split("@")[0]);

        if (user is null)
            throw new UserNotFoundException(nameof(user), userEmail);

        var role = await _roleManager.FindByNameAsync(roleName);

        if (role is null)
            throw new NotFoundException(nameof(role), roleName);

        await _userManager.AddToRoleAsync(user, roleName);

        return CustomResponseDto<NoDataDto>.Success(201);

    }

    public async Task<CustomResponseDto<List<AppUser>>> GetAllUsersByPage(string page)
    {
        var res = int.TryParse(page, out var intPage);
        if (!res)
            throw new ArgumentOutOfRangeException(nameof(page), page, "Page number is invalid");
        
        var users = await _userManager
            .Users
            .Skip(12*intPage)
            .Take(12)
            .ToListAsync();
        return CustomResponseDto<List<AppUser>>.Success(users, StatusCodes.Status200OK);
    }

    public async Task SendDeleteReqToBusinessApi(AppUser appUser)
    {
        using var client = new HttpClient();
        const string url = ApiConstants.BusinessApiIp + "/api/User/DeleteById";

        var requestData = new UserDeleteDto()
        {
            Id = appUser.Id,

        };

        var jsonData = JsonConvert.SerializeObject(requestData);

        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var clientToken = _authenticationService.CreateTokenByClient(
            new ClientLoginDto()
            {
                Id = _clientTokenOptions[0].Id,
                Secret = _clientTokenOptions[0].Secret
            }
        );
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken.Data.AccesToken);
        
        var response = await client.PostAsync(url, content);
        if (response.StatusCode != HttpStatusCode.OK)
            throw new OutOfReachException(url);
    }


}