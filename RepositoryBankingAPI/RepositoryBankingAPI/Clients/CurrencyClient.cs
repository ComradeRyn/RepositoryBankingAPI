using System.Net;
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
    
    public async Task<ApiResponse<CurrencyApiResponse>> GetConversionRatesAsync(string currencyTypes)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<CurrencyApiResponse>
                ($"v1/latest?apikey={_configuration["ApiKey"]}&currencies={currencyTypes}");

            return new ApiResponse<CurrencyApiResponse>(HttpStatusCode.OK, response, null);
        }
        catch (HttpRequestException e)
        {
            return new ApiResponse<CurrencyApiResponse>((HttpStatusCode)e.StatusCode!, null, e.Message);
        }
    }
}