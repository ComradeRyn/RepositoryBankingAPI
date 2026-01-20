using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Clients;

public class CurrencyClient
{
    private const string ApiWebAddress = "https://api.freecurrencyapi.com";
    private readonly HttpClient _sharedClient;
    private readonly IConfiguration _configuration;

    public CurrencyClient(IConfiguration configuration)
    {
        _configuration = configuration;
        _sharedClient = new HttpClient
        {
            BaseAddress = new Uri(ApiWebAddress),
        };
    }
    
    // TODO: cover the case if the CurrencyApi is down
    public async Task<CurrencyApiResponse?> GetConversionRatesAsync(string currencyTypes)
    {
        try
        {
            return await _sharedClient.GetFromJsonAsync<CurrencyApiResponse>
                ($"v1/latest?apikey={_configuration["ApiKey"]}&currencies={currencyTypes}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}