using RepositoryBankingAPI.Models;

namespace RepositoryBankingAPI.Interfaces;

public interface IAccountsRepository
{
    Task<Account?> GetAccount(string id);
    Task<Account> AddAccount(string name);
    Task<Account> UpdateAccount(Account account, decimal amount);
