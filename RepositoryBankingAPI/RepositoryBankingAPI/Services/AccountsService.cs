using System.Text.RegularExpressions;
using RepositoryBankingAPI.Models;
using RepositoryBankingAPI.Models.Records;

namespace RepositoryBankingAPI.Services;

public class AccountsService
{
    private const string NameRegexp = @"([A-Z][a-z]+)\s(([A-Z][a-z]*)\s)?([A-Z][a-z]+)";
    private readonly AccountContext _context;

    public AccountsService(AccountContext context)
    {
        _context = context;
    }

    public async Task<Account> GetAccount(string id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account is null)
        {
            // throw new AccountNotFoundException("No user found with given id", nameof(id));
        }
        
        return account;
    }

    public async Task<Account> CreateAccount(CreationRequest request)
    {
        ValidateName(request.Name);

        var newAccount = new Account
        {
            Id = Guid.NewGuid().ToString(),
            HolderName = request.Name,
        };
        
        await _context.Accounts.AddAsync(newAccount);
        await _context.SaveChangesAsync();
        
        return newAccount;
    }

    public async Task<UpdateBalanceResponse> Deposit(string id, ChangeBalanceRequest request)
    {
        var account = await GetAccount(id);
        
        ValidatePositiveAmount(request.Amount);
        
        account.Balance += request.Amount;
        
        await _context.SaveChangesAsync();
        
        return new UpdateBalanceResponse(account.Balance);
    }

    public async Task<UpdateBalanceResponse> Withdraw(string id, ChangeBalanceRequest request)
    {
        var account = await GetAccount(id);
        
        ValidatePositiveAmount(request.Amount);
        ValidateSufficientBalance(account.Balance, request.Amount);
        
        account.Balance -= request.Amount;

        await _context.SaveChangesAsync();
        
        return new UpdateBalanceResponse(account.Balance);
    }
    
    public async Task<UpdateBalanceResponse> Transfer(TransferRequest request)
    {
        var receiver = await GetAccount(request.ReceiverId);
        var sender = await GetAccount(request.SenderId);

        ValidatePositiveAmount(request.Amount);
        ValidateSufficientBalance(sender.Balance, request.Amount);

        sender.Balance -= request.Amount;
        receiver.Balance += request.Amount;
        
        await _context.SaveChangesAsync();

        return new UpdateBalanceResponse(receiver.Balance);
    }
    
    private void ValidateName(string name)
    {
        if (!Regex.IsMatch(name, NameRegexp))
        {
            throw new ArgumentException("Name must be in the format of <first name> <middle name> <last name>", 
                nameof(name));
        }
    }

    private void ValidatePositiveAmount(decimal amount)
    {
        if (amount <= 0)
        {
            // throw new NegativeAmountException("Requested amount must be positive", nameof(amount));
        }
    }

    private void ValidateSufficientBalance(decimal balance, decimal amount)
    {
        if (amount > balance)
        {
            // throw new InsufficientFundsException("Requested amount must be less than or equal to current balance",
            //    nameof(amount));
        }
    }
}