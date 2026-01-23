using System.Net;

namespace RepositoryBankingAPI.Models.DTOs.Responses;

public class ApiResponse<T>(HttpStatusCode statusCode, 
    T? content, 
    string? errorMessage)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public T? Content { get; } = content;
    public string? ErrorMessage { get; } = errorMessage;
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
}