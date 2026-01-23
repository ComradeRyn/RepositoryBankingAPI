using System.Net;
using System.Text.Json.Serialization;

namespace RepositoryBankingAPI.Models.DTOs.Responses;

public class CurrencyClientResponse
{
    public HttpStatusCode StatusCode { get; init; }
    public string? ErrorMessage { get; init; }
    [JsonPropertyName("data")]
    public Dictionary<string, decimal>? ConversionRates { get; init; }

    public CurrencyClientResponse(HttpStatusCode statusCode, 
        string? errorMessage,
        Dictionary<string, decimal>? conversionRates)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
        ConversionRates = conversionRates;
    }

    [JsonConstructor]
    public CurrencyClientResponse(Dictionary<string, decimal> conversionRates)
    {
        ConversionRates = conversionRates;
        StatusCode = HttpStatusCode.OK;
        ErrorMessage = null;
    }

}