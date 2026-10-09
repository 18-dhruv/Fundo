namespace ModelLayer.Models.Common
{
    /// <summary>
    /// Standard envelope returned by every API endpoint.
    /// </summary>
    public class ResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ResponseModel<T> Success(T data, string message = "Success") =>
            new() { IsSuccess = true, Message = message, Data = data };

        public static ResponseModel<T> SuccessWithoutData(string message) =>
            new() { IsSuccess = true, Message = message };

        public static ResponseModel<T> Failure(string message, T? data = default) =>
            new() { IsSuccess = false, Message = message, Data = data };
    }
}
