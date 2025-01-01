using System.Net;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Services;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.DTO.Exceptions;
using SharedLibrary.DTO.Result;

namespace DotnetNlayer.Service;

public class DummyEmailConfirmationService:IEmailConfirmationService
{
    private readonly UserManager<AppUser> _userManager;

    public DummyEmailConfirmationService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CustomResponseDto<NoDataDto>> ConfirmEmailAsync(string email, string token)
    {
        var user =await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new NotFoundException(nameof(AppUser), email);

        user.EmailConfirmed = true;
        return CustomResponseDto<NoDataDto> .Success((int)HttpStatusCode.OK);
    }

    public Task<string> GenerateEmailConfirmationTokenAsync(string email)
    {
        return Task.FromResult(Guid.NewGuid().ToString());
    }
}