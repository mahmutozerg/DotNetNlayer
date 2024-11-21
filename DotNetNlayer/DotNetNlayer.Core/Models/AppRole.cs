
using Microsoft.AspNetCore.Identity;

namespace DotNetNlayer.Core.Models;

public class AppRole(string name) : IdentityRole(name);