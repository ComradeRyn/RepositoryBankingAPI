using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Clients;

public interface ICurrencyClient
{
    public Task<ApiResponse<ConversionResponse>> GetConversionRates(string currencyTypes);
}