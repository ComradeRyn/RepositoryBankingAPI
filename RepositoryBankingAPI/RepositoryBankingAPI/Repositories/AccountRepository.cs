using RepositoryBankingAPI.Models;

namespace RepositoryBankingAPI.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AccountContext _context;

    public AccountRepository(AccountContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetAccount(string id)
        => await _context.Accounts.FindAsync(id);

    public async Task<Account> AddAccount(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();

        return account;
    }

    public async Task<Account?> UpdateAccount(Account account, decimal amount)
    {
        account.Balance += amount;
        await _context.SaveChangesAsync();

        return account;
    }
}