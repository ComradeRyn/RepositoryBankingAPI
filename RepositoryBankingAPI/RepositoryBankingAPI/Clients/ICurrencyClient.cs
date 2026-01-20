using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Clients;

public interface ICurrencyClient
{
    public Task<ApiResponse<ConversionResponse>> GetConversionRatesAsync(string currencyTypes);
}