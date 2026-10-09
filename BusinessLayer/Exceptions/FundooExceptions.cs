namespace BusinessLayer.Exceptions
{
    /// <summary>
    /// Base for every expected error. The API's ExceptionMiddleware turns
    /// StatusCode + Message into the HTTP response, so controllers need no try/catch.
    /// </summary>
    public class FundooException : Exception
    {
        public int StatusCode { get; }

        public FundooException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class BadRequestException : FundooException
    {
        public BadRequestException(string message) : base(message, 400) { }
    }

    public class UnauthorizedException : FundooException
    {
        public UnauthorizedException(string message) : base(message, 401) { }
    }

    public class NotFoundException : FundooException
    {
        public NotFoundException(string message) : base(message, 404) { }
    }

    public class ConflictException : FundooException
    {
        public ConflictException(string message) : base(message, 409) { }
    }

    public class ServiceUnavailableException : FundooException
    {
        public ServiceUnavailableException(string message) : base(message, 503) { }
    }
}
