using System.Net;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Services;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.DTO.Exceptions;
using SharedLibrary.DTO.Result;

namespace DotnetNlayer.Service.Services;

public class EmailConfirmationService:IEmailConfirmationService
{
    private readonly UserManager<AppUser> _userManager;

    public EmailConfirmationService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CustomResponseDto<NoDataDto>> ConfirmEmailAsync(string email,string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        token = WebUtility.UrlDecode(token);
        
        if (user == null)
            throw new UserNotFoundException(nameof(user),$"User with email {email}  not found");
        

        
        var isTokenValid = await _userManager.ConfirmEmailAsync(user, token);
        if (!isTokenValid.Succeeded)
            throw new InvalidParameterException("Email confirmation", $"User with email {email} and with token {token} not found");
        
        
        var result =await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new SomethingWentWrongException("Email confirmation", string.Join("", result.Errors.Select(x=> x.Description).ToList()));
        
        return CustomResponseDto<NoDataDto> .Success((int)HttpStatusCode.OK);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        
        if (user == null)
            throw new UserNotFoundException(nameof(email), email);

        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        if (isEmailConfirmed)
            throw new InvalidOperationException($"Email {email} is already confirmed");
        
        var userEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        if (string.IsNullOrEmpty(userEmailToken))
            throw new SomethingWentWrongException(nameof(email),"Something went wrong while creating email verification token");

        return Uri.EscapeDataString(userEmailToken);

    }
}