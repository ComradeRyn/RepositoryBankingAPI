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
    // TODO: check over service class for redundancy and adherence to conventions
    // TODO: review names in the DTOs for redundancy
    // TODO: Remove round trips to the database (see more in the repository class)
    
    private const string NameRegexp = @"([A-Z][a-z]+)\s(([A-Z][a-z]*)\s)?([A-Z][a-z]+)";
    private readonly IAccountRepository _repo;
    private readonly ICurrencyClient _client;

    public AccountsService(IAccountRepository repo, ICurrencyClient client)
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

    public async Task<ApiResponse<ChangeBalanceResponse>> Deposit(ApiRequest<ChangeBalanceRequest> request)
    {
        var account = await _repo.GetAccount(request.Id);

        if (account is null)
        {
            return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.NotFound, null, 
                Messages.NotFound);
        }

        if (request.Request.Amount <= 0)
        {
            return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.BadRequest, null,
                Messages.RequirePositiveAmount);
        }
        
        account.Balance += request.Request.Amount;

        await _repo.UpdateAccount(account);

        // TODO: take another look here for naming
        return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.OK, account.Balance.AsDto(), 
            null);
    }

    public async Task<ApiResponse<ChangeBalanceResponse>> Withdraw(ApiRequest<ChangeBalanceRequest> request)
    {
        var account = await _repo.GetAccount(request.Id);

        if (account is null)
        {
            return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.NotFound, null, 
                Messages.NotFound);
        }

        if (request.Request.Amount <= 0)
        {
            return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.BadRequest, null,
                Messages.RequirePositiveAmount);
        }

        if (request.Request.Amount > account.Balance)
        {
            return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.BadRequest, null,
                Messages.InsufficientBalance);
        }
        
        account.Balance -= request.Request.Amount;
        await _repo.UpdateAccount(account);
        
        return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.OK, account.Balance.AsDto(),
            null);
    }
    
    public async Task<ApiResponse<ChangeBalanceResponse>> Transfer(TransferRequest request)
    {
        var receiver = await _repo.GetAccount(request.ReceiverId);
        if (receiver is null)
        {
            return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.NotFound, null, 
                Messages.NotFound);
        }
        
        var sender = await _repo.GetAccount(request.SenderId);
        if (sender is null)
        {
            return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.NotFound, null, 
                Messages.NotFound);
        }

        if (request.Amount <= 0)
        {
            return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.BadRequest, null,
                Messages.RequirePositiveAmount);
        }

        if (request.Amount > sender.Balance)
        {
            return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.BadRequest, null,
                Messages.InsufficientBalance);
        }

        sender.Balance -= request.Amount;
        receiver.Balance += request.Amount;
        
        await _repo.UpdateAccount(sender);
        await _repo.UpdateAccount(receiver);

        return new ApiResponse<ChangeBalanceResponse>(HttpStatusCode.OK, receiver.Balance.AsDto(), 
            null);
    }
    
    public async Task<ApiResponse<CurrencyApiResponse>> Convert(ApiRequest<ConversionRequest> request)
    {
        var account = await _repo.GetAccount(request.Id);
        if (account is null)
        {
            return new ApiResponse<CurrencyApiResponse>(HttpStatusCode.NotFound, null, 
                Messages.NotFound); 
        }

        var response = await _client.GetConversionRatesAsync(request.Request.CurrencyType);
        if (response.Response is null)
        {
            return response;
        }

        foreach (var currencyType in response.Response.Data.Keys)
        {
            response.Response.Data[currencyType] *= account.Balance;
        }

        return response;
    }

    private bool ValidateName(string name)
        => Regex.IsMatch(name, NameRegexp);
    
}