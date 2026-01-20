using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Clients;

public class CurrencyClient
{
    private const string ApiWebAddress = "https://api.freecurrencyapi.com";
    private readonly HttpClient _sharedClient;
    
    // TODO: Remove this reference to the key and place it in the appsettings
    private readonly string key = "fca_live_6P8f9slpZdyzX8XxZrKPMb2EuzCttCd892zZnK1A";

    public CurrencyClient()
    {
        _sharedClient = new()
        {
            BaseAddress = new Uri(ApiWebAddress),
        };
    }

    // TODO: Must convert the JSON into a list of decimals
    public async Task<CurrencyApiResponse?> GetConversionRatesAsync(string currencyTypes)
    {
        try
        {
            return await _sharedClient.GetFromJsonAsync<CurrencyApiResponse>
                ($"v1/latest?apikey={key}&currencies={currencyTypes}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}