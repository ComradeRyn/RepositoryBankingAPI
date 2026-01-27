using System.Net;

namespace RepositoryBankingAPI.Models.DTOs.Responses;

public class ApiResponse<T>(HttpStatusCode statusCode, T? content, string? errorMessage)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public T? Content { get; } = content;
    public string? ErrorMessage { get; } = errorMessage;
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    
    public ApiResponse(HttpStatusCode statusCode, string errorMessage) 
        : this(statusCode, default, errorMessage) { }

    public ApiResponse(T content) 
        : this(HttpStatusCode.OK, content, null) { }
}