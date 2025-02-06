using System.Net;
using System.Security.Claims;
using System.Text;
using DotNetNlayer.Core.Constants;
using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.DTO.Tokens;
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
using SharedLibrary.Constants;
using SharedLibrary.Constants.Response;
using SharedLibrary.DTO.Exceptions;
using SharedLibrary.DTO.Result;

namespace DotnetNlayer.Service.Services;

public class AppUserService : GenericService<AppUser>, IAppUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppAuthenticationService _appAuthenticationService;
    private readonly List<ClientLoginDto> _clientTokenOptions;
    private readonly  ITokenService _tokenService;
    public AppUserService(UserManager<AppUser> userManager, IAppUserRepository repository, IUnitOfWork unitOfWork,IAppAuthenticationService appAuthenticationService,
        IOptions<List<ClientLoginDto>> clientTokenOptions, ITokenService tokenService) : base(repository, unitOfWork)
    {
        _userManager = userManager;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _appAuthenticationService = appAuthenticationService;
        _tokenService = tokenService;
        _clientTokenOptions = clientTokenOptions.Value;
    }


    public async Task<CustomResponseDto<AppUser>> CreateAsync(AppUserCreateDto createAppUserDto)
    {
        ComparePassword(createAppUserDto);
        var emailResult = await _userManager.FindByEmailAsync(createAppUserDto.Email) ;
        if(emailResult != null)
            return CustomResponseDto<AppUser>
                .Fail("Failed to create user",
                    (int)HttpStatusCode.BadRequest, 
                    $"Email address '{createAppUserDto.Email}' is taken");
        
        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(), 
            Email = createAppUserDto.Email,
            UserName = createAppUserDto.UserName,
            CreatedAt = DateTime.Now, 
            CreatedBy = "System",
            UpdatedAt = DateTime.Now,
            UpdatedBy = "System",
        };
        
        var result = await _userManager.CreateAsync(user, createAppUserDto.Password);

        if (!result.Succeeded)
        {
            return CustomResponseDto<AppUser>
                .Fail("Failed to create user",
                    (int)HttpStatusCode.BadRequest, 
                    string.Join("",result.Errors.Select(x => x.Description).ToList()));
        }

        /*
         * This is for creating the same user in business database
         * when a new user created we are sending the request to the business api
         * For that we need a bearer token(client token) which we are going to get from authentication service
         * User/AddById endpoint is authorized with a policy according to that so only a authserver client can reach there
         */
        
        
         var content =await SendReqToBusinessApiAddById(user);

         if (content  == string.Empty)
             return CustomResponseDto<AppUser>.Success(user, 200);


         await _userManager.DeleteAsync(user);
         throw new SomethingWentWrongException("Failed to create user due to Business api error",content);
        
    }

    public async Task<CustomResponseDto<AppUser>> GetByIdAsync(string userId)
    {
        var user = await _repository.Where(user => user!.Id == userId).SingleOrDefaultAsync();
        
        if (user == null)
            throw new UserNotFoundException(nameof(user), userId);

        return CustomResponseDto<AppUser>.Success(user, 200);
    }



    public async Task<CustomResponseDto<AppUser>> GetByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return user is null ? 
            throw new UserNotFoundException(nameof(user),userName) 
            : CustomResponseDto<AppUser>.Success(user,StatusCodes.Status200OK);
    }

    public async Task<CustomResponseDto<AppUser>> GetByEmailAsync(string eMail)
    {
        var user = await _userManager.FindByEmailAsync(eMail);
        return user is null ? 
            throw new UserNotFoundException(nameof(user), eMail)
            : CustomResponseDto<AppUser>.Success(user, 200);
    }

    public async Task<CustomResponseDto<NoDataDto>> RemoveAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            throw new UserNotFoundException(nameof(user), id);

        _repository.Remove(user);
        await _unitOfWork.SaveChangesAsync();

        await SendDeleteReqToBusinessApiAsync(user);
        return CustomResponseDto<NoDataDto>.Success(200);
    }
    
    public async Task<CustomResponseDto<NoDataDto>> UpdateAsync(AppUserUpdateDto userUpdateDto, ClaimsIdentity userIdentity)
    {
        var userId = userIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if(userId is null)
            throw new SomethingWentWrongException(nameof(userIdentity),$"UserId not found at JWT token");

        var existingUserById = await _userManager.FindByIdAsync(userId);
        
        if(existingUserById is null)
            throw new UserNotFoundException(nameof(existingUserById), userId);

        if (existingUserById.Email != userUpdateDto.Email)
        {
            var requestedEmailUser = await  _userManager.FindByEmailAsync(userUpdateDto.Email);
            if (requestedEmailUser is not null)
                throw new AlreadyExistException(nameof(requestedEmailUser), userUpdateDto.Email);
            
            existingUserById.Email = userUpdateDto.Email;

        }

        if (existingUserById.UserName == userUpdateDto.UserName) 
            return CustomResponseDto<NoDataDto>.Success(201);
        
        
        var requestedUserNameUser = await  _userManager.FindByNameAsync(userUpdateDto.UserName);
        if (requestedUserNameUser is not null)
            throw new AlreadyExistException(nameof(requestedUserNameUser), userUpdateDto.Email);

        return CustomResponseDto<NoDataDto>.Success(201);


    }

    public async Task<CustomResponseDto<TokenDto>> UpdatePasswordAsync(AppUserUpdatePasswordDto userUpdatePasswordDto, ClaimsIdentity claimsIdentity)
    {
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            throw new UserNotFoundException("UserId", string.Empty);
        
        var userEntity = await _userManager.FindByIdAsync(userId);

        var passwordChangeResult = await _userManager.ChangePasswordAsync(userEntity,userUpdatePasswordDto.OldPassword,userUpdatePasswordDto.NewPassword);

        if (!passwordChangeResult.Succeeded)
            return CustomResponseDto<TokenDto>
                .Fail(ResponseMessages.UpdatePasswordMissMatch,
                    (int)HttpStatusCode.BadRequest,
                    string.Join("",passwordChangeResult.Errors.Select(e=> e.Description)));
        
        var token = await _tokenService.CreateTokenAsync(userEntity);
            
        return CustomResponseDto<TokenDto>.Success(token,202);


    }
    
    private static void ComparePassword(AppUserCreateDto appUserCreateDto)
    {
        if (String.CompareOrdinal(appUserCreateDto.Password, appUserCreateDto.ConfirmPassword) != 0)
            throw new UserCreatePasswordMatchException(nameof(AppUserCreateDto.Password),
                appUserCreateDto.Password, 
                nameof(appUserCreateDto.ConfirmPassword),
                appUserCreateDto.ConfirmPassword);
    }
    
    private async Task<string> SendReqToBusinessApiAddById(AppUser user)
    {
        return string.Empty;
        using var client = new HttpClient();
        
        const string url = ApiConstants.BusinessApiIp + "/api/User/AddById";

        var requestData = new AppUserAddToBusinessApiDto()
        {
            Id = user.Id,
            Email = user.Email,

        };

        var jsonData = JsonConvert.SerializeObject(requestData);

        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var clientToken = _appAuthenticationService.CreateTokenByClient(
            new ClientLoginDto()
            {
                Id = _clientTokenOptions[0].Id,
                Secret = _clientTokenOptions[0].Secret
            }
        );
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken.Data.AccesToken);
        
        var response = await client.PostAsync(url, content);

        return response.StatusCode == HttpStatusCode.Created ? string.Empty : response.Content.ReadAsStringAsync().Result;
    }
    public async Task SendDeleteReqToBusinessApiAsync(AppUser appUser)
    {
        throw new NotImplementedException("SendDeleteReqToBusinessApiAsync not implemented");
        using var client = new HttpClient();
        const string url = ApiConstants.BusinessApiIp + "/api/User/DeleteById";

        var requestData = new AppUserDeleteDto()
        {
            Id = appUser.Id,

        };

        var jsonData = JsonConvert.SerializeObject(requestData);

        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        
        var clientToken = _appAuthenticationService.CreateTokenByClient(
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