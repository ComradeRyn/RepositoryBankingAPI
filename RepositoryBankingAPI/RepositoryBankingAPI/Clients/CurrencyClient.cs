using Microsoft.EntityFrameworkCore.Infrastructure;
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
    
    // TODO: cover the case if the CurrencyApi is down
    public async Task<CurrencyApiResponse?> GetConversionRatesAsync(string currencyTypes)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CurrencyApiResponse>
                ($"v1/latest?apikey={_configuration["ApiKey"]}&currencies={currencyTypes}");
        }
        catch (HttpRequestException e)
        {
            // if (e.StatusCode.)
            // {
            //     
            // }
            return null;
        }
    }
}