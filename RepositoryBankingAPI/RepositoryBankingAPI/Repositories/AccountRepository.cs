using RepositoryBankingAPI.Models;

namespace RepositoryBankingAPI.Repositories;

public class AccountRepository : IAccountRepository
{
    // TODO: revise Update function to prevent round trips
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

    public async Task<Account?> UpdateAccount(Account account)
    {
        var result = await GetAccount(account.Id);
        if (result is null)
        {
            return null;
        }

        result.Balance = account.Balance;
        await _context.SaveChangesAsync();

        return result;
    }
}