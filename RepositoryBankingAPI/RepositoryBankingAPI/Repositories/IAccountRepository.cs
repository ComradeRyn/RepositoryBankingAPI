using RepositoryBankingAPI.Models;

namespace RepositoryBankingAPI.Repositories;

public interface IAccountRepository
{
    public Task<Account?> GetAccount(string id);
    public Task<Account> AddAccount(Account account);
    public Task<Account?> UpdateAccount(Account account, decimal amount);
}