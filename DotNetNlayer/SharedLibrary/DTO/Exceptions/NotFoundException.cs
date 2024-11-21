using System.Net;

namespace SharedLibrary.DTO.Exceptions;

public class NotFoundException:CustomBaseException
{
    public override string MessageFormat =>  "{propName} : not found any records with/for '{propValue}' value";
    public override string Title => "Not Found";
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
   
    public NotFoundException(string propName, string propValue): base()
    {
        MessageProps.Add("{propName}", propName);
        MessageProps.Add("{propValue}", propValue);
    }
}