using System.Text.Json;
using RepositoryBankingAPI.Interfaces;
using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Clients;

public class CurrencyClient : ICurrencyClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CurrencyClient(HttpClient httpClient, IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }
    
    public async Task<CurrencyClientResponse> GetConversionRates(string currencyTypes)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"v1/latest?apikey={_configuration["ApiKey"]}&currencies={currencyTypes}");
        
        using var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return new CurrencyClientResponse(response.StatusCode, response.ReasonPhrase, null);
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<CurrencyClientResponse>(responseContent)!;
    }
}