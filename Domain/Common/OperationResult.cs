namespace Domain.Common
{
    public class OperationResult <T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public IEnumerable<string>? Errors { get; set; } 
        public T? Data { get; set; }

        public static OperationResult<T> Success(T data) =>
            new() { IsSuccess = true, Data = data };

        // For single, general errors
        public static OperationResult<T> Failure(string errorMessage) =>
            new() { IsSuccess = false, ErrorMessage = errorMessage ,  Errors = new List<string> { errorMessage } };

        // For multiple validation errors from FluentValidation
        public static OperationResult<T> Failure(string[] errors) =>
            new() { IsSuccess = false, Errors = errors };
    }
}
