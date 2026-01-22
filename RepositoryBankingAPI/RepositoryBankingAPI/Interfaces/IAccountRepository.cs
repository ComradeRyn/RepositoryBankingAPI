using RepositoryBankingAPI.Models;

namespace RepositoryBankingAPI.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetAccount(string id);
    Task<Account> AddAccount(Account account);
    Task<Account> UpdateAccount(Account account, decimal amount);
}