using System.Net;

namespace SharedLibrary.DTO.Exceptions;

public class SomethingWentWrongException:CustomBaseException
{
    public override string MessageFormat =>  "{propName} {propValue] ";
    public override string Title => "Something went wrong";
    public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
   
    public SomethingWentWrongException(string propName, string propValue): base()
    {
        MessageProps.Add("{propName}", propName);
        MessageProps.Add("{propValue}", propValue);

    }
}