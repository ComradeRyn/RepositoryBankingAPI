using RepositoryBankingAPI.Models.DTOs.Responses;
using Account = RepositoryBankingAPI.Models.Account;

namespace RepositoryBankingAPI.Services;

public static class DtoMappers
{
    public static Models.DTOs.Account AsDto(this Account account) 
        => new(account.Id, account.HolderName, account.Balance);
}