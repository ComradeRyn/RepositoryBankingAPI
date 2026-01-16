namespace RepositoryBankingAPI.Clients;

public class CurrencyClient
{
    private const string ApiWebAddress = "https://api.freecurrencyapi.com";
    private readonly HttpClient _sharedClient;
    private readonly string key = "fca_live_6P8f9slpZdyzX8XxZrKPMb2EuzCttCd892zZnK1A";

    public CurrencyClient()
    {
        _sharedClient = new()
        {
            BaseAddress = new Uri(ApiWebAddress),
        };
    }

    // TODO: Must convert the JSON into a list of decimals
    public async Task<List<decimal>?> GetConversionRatesAsync(string currencyTypes)
    {
        var response = await _sharedClient.GetFromJsonAsync<List<decimal>>
                ($"v1/latest?apikey={key}&currencies={currencyTypes}");

        return response;
    }
}