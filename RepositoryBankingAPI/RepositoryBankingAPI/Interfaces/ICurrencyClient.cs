using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Interfaces;

public interface ICurrencyClient
{
    Task<CurrencyClientResponse> GetConversionRates(string currencyTypes);
}