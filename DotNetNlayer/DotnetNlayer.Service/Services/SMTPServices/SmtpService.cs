using DotNetNlayer.Core.Services.SMTPServices;

namespace DotnetNlayer.Service.Services.SMTPServices;

public class SmtpService:ISmtpService
{


    public Task SendConfirmationEmailAsync(string email, string subject, string message)
    {
        return Task.CompletedTask;
        
    }
}