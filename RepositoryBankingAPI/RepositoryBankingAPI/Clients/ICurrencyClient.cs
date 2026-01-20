using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Clients;

public interface ICurrencyClient
{
    public Task<ApiResponse<CurrencyApiResponse>> GetConversionRatesAsync(string currencyTypes);
}