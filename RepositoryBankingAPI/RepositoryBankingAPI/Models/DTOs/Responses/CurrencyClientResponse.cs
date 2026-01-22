using System.Net;

namespace RepositoryBankingAPI.Models.DTOs.Responses;

public record CurrencyClientResponse(HttpStatusCode StatusCode, 
    string? ErrorMessage, 
    Dictionary<string, decimal>? ConversionRates);