using System.Net;
using System.Text.Json.Serialization;

namespace RepositoryBankingAPI.Models.DTOs.Responses;

public class CurrencyClientResponse(
    HttpStatusCode statusCode,
    string? errorMessage,
    Dictionary<string, decimal>? conversionRates)
{
    public HttpStatusCode StatusCode { get; init; } = statusCode;
    public string? ErrorMessage { get; init; } = errorMessage;
    [JsonPropertyName("data")]
    public Dictionary<string, decimal>? ConversionRates { get; init; } = conversionRates;
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    
    [JsonConstructor]
    public CurrencyClientResponse(Dictionary<string, decimal> conversionRates) 
        : this(HttpStatusCode.OK, null, conversionRates)
    { }

}