using DotNetNlayer.Core.Services.SMTPServices;

namespace DotnetNlayer.Service.Services.SMTPServices;

public class SmtpService:ISmtpService
{


    public Task SendConfirmationEmailAsync(string email, string subject, string message)
    {
        Console.WriteLine($"{email}: {subject}: {message}");
        return Task.CompletedTask;
        
    }
}