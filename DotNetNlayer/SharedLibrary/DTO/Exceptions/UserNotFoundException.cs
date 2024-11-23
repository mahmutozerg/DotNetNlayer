using System.Net;

namespace SharedLibrary.DTO.Exceptions;

public class UserNotFoundException:CustomBaseException
{
    public override string MessageFormat =>  "{propName} : for '{propValue}' value we couldn't find the user.";
    public override string Title => "User Not Found";
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
   
    public UserNotFoundException(string propName, string propValue): base()
    {
        MessageProps.Add("{propName}", propName);
        MessageProps.Add("{propValue}", propValue);
    }
    
}