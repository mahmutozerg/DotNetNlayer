namespace DotNetNlayer.Core.Services.SMTPServices;

public interface ISmtpService
{
    public Task SendConfirmationEmailAsync(string email, string subject, string message);
}