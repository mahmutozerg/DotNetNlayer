using System.Net;

namespace SharedLibrary.DTO.Exceptions;

public class UserCreatePasswordMatchException:CustomBaseException
{
    
    public override string MessageFormat =>  "{propName} should match with {propName2} : {propValue] does not match {propValue2}";
    public override string Title => "Confirm Password and Password does not match";
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
   
    public UserCreatePasswordMatchException(string propName, string propValue,string propName2,string propValue2): base()
    {
        MessageProps.Add("{propName}", propName);
        MessageProps.Add("{propValue}", propValue);
        MessageProps.Add("{propName2}", propName2);
        MessageProps.Add("{propValue2}", propValue2);
    }
}