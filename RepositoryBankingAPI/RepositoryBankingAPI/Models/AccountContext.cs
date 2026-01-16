using Microsoft.EntityFrameworkCore;

namespace RepositoryBankingAPI.Models;

public class AccountContext(DbContextOptions<AccountContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; init; }
}