using System.Net;

namespace SharedLibrary.DTO.Exceptions;

public class OutOfReachException:CustomBaseException
{
    public override string MessageFormat =>  "The api with '{propValue}' value is out of reach.";
    public override string Title => "out of reach";
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
   
    public OutOfReachException( string propValue): base()
    {
        MessageProps.Add("{propValue}", propValue);
    }
}