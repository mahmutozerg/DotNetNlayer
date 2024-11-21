namespace DotNetNlayer.Core.Configurations;

/// <summary>
///  The Class that will represent our client (applications)
/// ID = xxxxx-xxxxx-xxxx-xxxx
/// Secret = Client's  Secret key  its like a password
/// Audiences = Urls that our Client should be able to access (Doesn't used in this project)
/// </summary>

public class Client
{
    public string Id { get; set; } = string.Empty;

    public string Secret { get; set; } = string.Empty;

    public List<String> Audiences { get; set; } = new();
}