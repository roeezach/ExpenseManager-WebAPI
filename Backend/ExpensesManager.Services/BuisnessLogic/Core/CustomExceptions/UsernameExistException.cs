using System.Runtime.Serialization;

namespace ExpensesManager.Services;
public class UsernameExistException : Exception
{
    public UsernameExistException()
    {
    }

    public UsernameExistException(string message) : base(message)
    {
    }

    public UsernameExistException(string message, Exception innerException)
    : base(message, innerException)
    {
    }

    protected UsernameExistException(SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
    }
}
