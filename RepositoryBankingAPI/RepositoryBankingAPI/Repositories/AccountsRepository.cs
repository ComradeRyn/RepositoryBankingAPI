using RepositoryBankingAPI.Interfaces;
using RepositoryBankingAPI.Models;

namespace RepositoryBankingAPI.Repositories;

public class AccountsesRepository : IAccountsRepository
{
    private readonly AccountContext _context;

    public AccountsesRepository(AccountContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetAccount(string id)
        => await _context.Accounts.FindAsync(id);

    public async Task<Account> AddAccount(string name)
    {
        var account = new Account
        {
            Id = Guid.NewGuid().ToString(),
            HolderName = name,
        };
        
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();

        return account;
    }

    public async Task<Account> UpdateAccount(Account account, decimal amount)
    {
        account.Balance += amount;
        await _context.SaveChangesAsync();

        return account;
    }
}