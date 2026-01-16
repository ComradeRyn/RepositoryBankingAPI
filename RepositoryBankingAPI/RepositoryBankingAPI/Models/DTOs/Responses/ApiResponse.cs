using System.Net;

namespace RepositoryBankingAPI.Models.DTOs.Responses;

public record ApiResponse<T>(HttpStatusCode StatusCode, T? Response, string? ErrorMessage);