using System.Net;

namespace SharedLibrary.DTO.Exceptions;

public class InvalidParameterException: CustomBaseException
{
    public override string MessageFormat => "Invalıd Parameter . {fieldName} : {fieldValue}";
    public override string Title => "Invalid Parameter Error";
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public InvalidParameterException(string fieldName, string fieldValue) : base()
    {
        MessageProps.Add("{fieldName}", fieldName);
        MessageProps.Add("{fieldValue}", fieldValue);
    }
    
}