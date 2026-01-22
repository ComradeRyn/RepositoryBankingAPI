using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Interfaces;

public interface ICurrencyClient
{
    Task<ApiResponse<ConversionResponse>> GetConversionRates(string currencyTypes);
}