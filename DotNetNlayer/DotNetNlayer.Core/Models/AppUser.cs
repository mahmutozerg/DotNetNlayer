using Microsoft.AspNetCore.Identity;

namespace DotNetNlayer.Core.Models;

public class AppUser:IdentityUser
{
    public DateTime? CreatedAt { get; set; } 
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    
}