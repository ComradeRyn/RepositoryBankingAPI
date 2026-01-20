using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Clients;

public interface ICurrencyClient
{
    public Task<CurrencyApiResponse?> GetConversionRatesAsync(string currencyTypes);
}