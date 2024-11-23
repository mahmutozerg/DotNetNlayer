
using Microsoft.AspNetCore.Identity;

namespace DotNetNlayer.Core.Models;

public sealed class AppRole(string name) : IdentityRole(name);