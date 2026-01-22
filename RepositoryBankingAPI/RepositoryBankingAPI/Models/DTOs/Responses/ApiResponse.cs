using System.Net;

namespace RepositoryBankingAPI.Models.DTOs.Responses;

public class ApiResponse<T>(HttpStatusCode statusCode, T? content, string? errorMessage)
{
    public HttpStatusCode StatusCode { get; init; } = statusCode;
    public T? Content { get; init; } = content;
    public string? ErrorMessage { get; init; } = errorMessage;
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
}
    