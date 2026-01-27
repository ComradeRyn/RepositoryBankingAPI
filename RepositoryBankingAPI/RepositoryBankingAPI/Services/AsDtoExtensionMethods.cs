using RepositoryBankingAPI.Models.DTOs.Responses;
using Account = RepositoryBankingAPI.Models.Account;

namespace RepositoryBankingAPI.Services;

public static class AsDtoExtensionMethods
{
    public static Models.DTOs.Account AsDto(this Account account) 
        => new(account.Id, account.HolderName, account.Balance);
    public static ConversionResponse AsDto(this Dictionary<string, decimal> convertedCurrencies)
        => new(convertedCurrencies);
}