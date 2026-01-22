using System.Net;
using RepositoryBankingAPI.Models;
using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Services;

public static class ExtensionMethods
{
    public static AccountResponse AsDto(this Account account) 
        => new(account.Id, account.HolderName, account.Balance);
    public static ChangeBalanceResponse AsDto(this decimal amount) 
        => new(amount);
    public static bool ValidateSuccessfulCode<T>(this ApiResponse<T> response)
        => response.StatusCode is >= (HttpStatusCode)200 and < (HttpStatusCode)300;
}