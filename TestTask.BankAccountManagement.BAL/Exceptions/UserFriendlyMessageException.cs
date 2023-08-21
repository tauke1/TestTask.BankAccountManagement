using System.Net;

namespace TestTask.BankAccountManagement.BAL.Exceptions
{
    public class UserFriendlyMessageException : Exception
    {
        public virtual HttpStatusCode StatusCode { get; }

        public UserFriendlyMessageException(
            string message) : base(message)
        {
        }

        public UserFriendlyMessageException(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            StatusCode = statusCode;
        }

        public UserFriendlyMessageException(
            string message,
            Exception innerException,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
