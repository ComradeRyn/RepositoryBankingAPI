using System.Net;
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
    
    public async Task<ApiResponse<ConversionResponse>> GetConversionRates(string currencyTypes)
    {
        // try
        // {
        //     var response = await _httpClient
        //         .GetFromJsonAsync<ConversionResponse>
        //             ($"v1/latest?apikey={_configuration["ApiKey"]}&currencies={currencyTypes}");
        //
        //     return new ApiResponse<ConversionResponse>(HttpStatusCode.OK, response, null);
        // }
        // catch (HttpRequestException e)
        // {
        //     return new ApiResponse<ConversionResponse>((HttpStatusCode)e.StatusCode!, null, e.Message);
        // }
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"v1/latest?apikey={_configuration["ApiKey"]}&currencies={currencyTypes}");
        using var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            // Return some kind of error
        }

        // Return some kind of not error
        return null;
    }
}