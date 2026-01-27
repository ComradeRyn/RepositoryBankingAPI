using System.Net;

namespace RepositoryBankingAPI.Models.DTOs.Responses;

public class ApiResponse<T>
{
    public HttpStatusCode StatusCode { get; }
    public T? Content { get; }
    public string? ErrorMessage { get; }
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    
    public ApiResponse(HttpStatusCode statusCode, string errorMessage)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
        Content = default;
    }

    public ApiResponse(T content)
    {
        StatusCode = HttpStatusCode.OK;
        Content = content;
        ErrorMessage = null;
    }
}