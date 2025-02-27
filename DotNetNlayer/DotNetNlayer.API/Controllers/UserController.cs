using System.Security.Claims;
using DotNetNlayer.Core.DTO.User;
using DotNetNlayer.Core.Services;
using DotNetNlayer.Core.Services.SMTPServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
namespace DotNetNlayer.API.Controllers;

[Route("/[controller]/[action]")]
[ApiController]
public class UserController:ControllerBase
{
    private readonly IAppUserService _appUserService;
    private readonly ISmtpService _smtpService;
    private readonly IEmailConfirmationService _emailConfirmationService;
    
     public UserController(IAppUserService appUserService,  ISmtpService smtpService, IEmailConfirmationService emailConfirmationService)
     {
         _appUserService = appUserService;
         _smtpService = smtpService;
         _emailConfirmationService = emailConfirmationService;
     }


     [HttpGet]
     public async Task<IActionResult> ConfirmEmail(string email, string token)
     {
         
         return new ObjectResult(await _emailConfirmationService.ConfirmEmailAsync(email, token));     
     }

     [HttpGet]
     [Authorize]
     public async Task<IActionResult> RequestEmailConfirmation()
     {
         var userEmail = User.FindFirstValue(ClaimTypes.Email);
         
         var emailConfirmationToken = await _emailConfirmationService.GenerateEmailConfirmationTokenAsync(userEmail);
        
         return await ConfirmEmail( userEmail,emailConfirmationToken);
         
     }
     
    [HttpPost]
    public async Task<IActionResult?> CreateUser(AppUserCreateDto createAppUserDto)
    {
            
            
        var userResponse = (await _appUserService.CreateAsync(createAppUserDto)) ;
        
        var user = userResponse.Data;
        if (!userResponse.IsSuccess)
            return new ObjectResult(userResponse) ;
        
        var emailConfirmationToken = await _emailConfirmationService.GenerateEmailConfirmationTokenAsync(user.Email!);
        
        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var verificationUrl = $"{baseUrl}{Url.Action(nameof(ConfirmEmail), new { email = user.Email, token = emailConfirmationToken })}";

        
        await _smtpService.SendConfirmationEmailAsync(user.Email!, "Email Confirmation", verificationUrl!);

        return new ObjectResult(Ok());

    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        return  new ObjectResult(await _appUserService.GetByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {

        return  new ObjectResult(await _appUserService.RemoveAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)));
    }
    
    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateUser(AppUserUpdateDto userUpdateDto)
    {

        return  new ObjectResult(await _appUserService.UpdateAsync(userUpdateDto,(ClaimsIdentity)User.Identity));
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateUserPassword(AppUserUpdatePasswordDto userUpdateDto)
    {

        return  new ObjectResult(await _appUserService.UpdatePasswordAsync(userUpdateDto,(ClaimsIdentity)User.Identity));
    }
    
}