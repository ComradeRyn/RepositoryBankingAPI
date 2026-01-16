using System.Net;
using System.Text.RegularExpressions;
using RepositoryBankingAPI.Clients;
using RepositoryBankingAPI.Models;
using RepositoryBankingAPI.Models.DTOs.Requests;
using RepositoryBankingAPI.Models.DTOs.Responses;
using RepositoryBankingAPI.Repositories;

namespace RepositoryBankingAPI.Services;

public class AccountsService
{
    private const string NameRegexp = @"([A-Z][a-z]+)\s(([A-Z][a-z]*)\s)?([A-Z][a-z]+)";
    private readonly IAccountRepository _repo;
    private readonly CurrencyClient _client;

    public AccountsService(IAccountRepository repo, CurrencyClient client)
    {
        _repo = repo;
        _client = client;
    }

    public async Task<ApiResponse<AccountResponse>> GetAccount(string id)
    {
        var account = await _repo.GetAccount(id);
        if (account is null)
        {
            return new ApiResponse<AccountResponse>(HttpStatusCode.NotFound, null, 
                Messages.NotFound);
        }
        
        return new ApiResponse<AccountResponse>(HttpStatusCode.OK, account.AsDto(), null);
    }

    public async Task<ApiResponse<AccountResponse>> CreateAccount(CreationRequest request)
    {
        if (!ValidateName(request.Name))
        {
            return new ApiResponse<AccountResponse>(HttpStatusCode.BadRequest, null, 
                Messages.InvalidName);
        }

        var newAccount = new Account
        {
            Id = Guid.NewGuid().ToString(),
            HolderName = request.Name,
        };
        
        await _repo.AddAccount(newAccount);
        
        return new ApiResponse<AccountResponse>(HttpStatusCode.OK, newAccount.AsDto(), null);
    }

    public async Task<ApiResponse<ChangeBalanceResponse>> Deposit(ApiRequest<ChangeBalanceResponse> request)
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