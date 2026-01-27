using System.Net;
using System.Text.Json.Serialization;

namespace RepositoryBankingAPI.Models.DTOs.Responses;

public class CurrencyClientResponse(
    HttpStatusCode? errorCode,
    string? errorMessage,
    Dictionary<string, decimal>? conversionRates)
{
    public HttpStatusCode? ErrorCode { get; } = errorCode;
    public string? ErrorMessage { get; } = errorMessage;
    [JsonPropertyName("data")]
    public Dictionary<string, decimal>? ConversionRates { get; } = conversionRates;
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    
    [JsonConstructor]
    public CurrencyClientResponse(Dictionary<string, decimal> conversionRates) 
        : this(null, 
            null, 
            conversionRates) { }
}