using System.Net;
using RepositoryBankingAPI.Models.DTOs.Responses;
using Account = RepositoryBankingAPI.Models.DTOs.Responses.Account;

namespace RepositoryBankingAPI.Services;

public static class ExtensionMethods
{
    public static Account AsDto(this Models.Account account) 
        => new(account.Id, account.HolderName, account.Balance);
    public static ConversionResponse AsDto(this Dictionary<string, decimal> convertedCurrencies)
        => new(convertedCurrencies);
}