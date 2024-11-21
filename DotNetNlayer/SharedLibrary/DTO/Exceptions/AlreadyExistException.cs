using System.Net;

namespace SharedLibrary.DTO.Exceptions;

/// <summary>
///  Custom exception for already existing records
/// </summary>
public class AlreadyExistException:CustomBaseException
{
    public override string MessageFormat =>  "{propName} : there is already a record exist with '{propValue}' value";
    public override string Title => "Already Exist Error";
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
   
    public AlreadyExistException(string propName, string propValue): base()
    {
        MessageProps.Add("{propName}", propName);
        MessageProps.Add("{propValue}", propValue);
    }
}