using System.ComponentModel.DataAnnotations;

namespace DotNetNlayer.Core.Utils;

public static class ValidationUtils
{
    public static bool IsValidGuid(string input)
    {
        return Guid.TryParse(input, out Guid _);
    }

    public static bool IsValidEmail(string email)
    {
        var emailAttribute = new EmailAddressAttribute();
        return emailAttribute.IsValid(email);
    }
}