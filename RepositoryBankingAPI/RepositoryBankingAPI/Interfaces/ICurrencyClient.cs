using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Interfaces;

public interface ICurrencyClient
{
    public Task<ApiResponse<ConversionResponse>> GetConversionRates(string currencyTypes);
}