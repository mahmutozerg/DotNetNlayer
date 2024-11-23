using System.Net;

namespace SharedLibrary.DTO.Exceptions;

public class CustomBaseException:Exception
{
    

    public virtual string MessageFormat { get;}
    public virtual string Title  { get; }
    public virtual HttpStatusCode StatusCode  { get; }
    public virtual Dictionary<string, string> MessageProps {get; set;}=new();
}