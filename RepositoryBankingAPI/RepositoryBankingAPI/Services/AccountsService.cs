using System.Net;
using System.Text.RegularExpressions;
using RepositoryBankingAPI.Clients;
using RepositoryBankingAPI.Interfaces;
using RepositoryBankingAPI.Models;
using RepositoryBankingAPI.Models.Constants;
using RepositoryBankingAPI.Models.DTOs.Requests;
using RepositoryBankingAPI.Models.DTOs.Responses;
using RepositoryBankingAPI.Repositories;
using Account = RepositoryBankingAPI.Models.DTOs.Responses.Account;

namespace RepositoryBankingAPI.Services;

public class AccountsService
{
    private const string NameRegexp = @"([A-Z][a-z]+)\s(([A-Z][a-z]*)\s)?([A-Z][a-z]+)";
    private readonly IAccountRepository _repo;
    private readonly ICurrencyClient _client;

    public AccountsService(IAccountRepository repo, ICurrencyClient client)
    {
        _repo = repo;
        _client = client;
    }

    public async Task<ApiResponse<Account>> GetAccount(string id)
    {
        var account = await _repo.GetAccount(id);
        if (account is null)
        {
            return new ApiResponse<Account>(HttpStatusCode.NotFound,
                null,
                Messages.NotFound);
        }
        
        return new ApiResponse<Account>(HttpStatusCode.OK,
            account.AsDto(),
            null);
    }

    public async Task<ApiResponse<Account>> CreateAccount(CreationRequest request)
    {
        if (!ValidateName(request.Name))
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest,
                null,
                Messages.InvalidName);
        }

        var newAccount = new Models.Account
        {
            Id = Guid.NewGuid().ToString(),
            HolderName = request.Name,
        };
        await _repo.AddAccount(newAccount);
        
        return new ApiResponse<Account>(HttpStatusCode.OK, 
            newAccount.AsDto(), 
            null);
    }

    public async Task<ApiResponse<Account>> Deposit(ApiRequest<ChangeBalanceRequest> request)
    {
        var account = await _repo.GetAccount(request.Id);
        if (account is null)
        {
            return new ApiResponse<Account>(HttpStatusCode.NotFound,
                null,
                Messages.NotFound);
        }

        if (request.Content.Amount <= 0)
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest,
                null, 
                Messages.NoNegativeAmount);
        }

        await _repo.UpdateAccount(account, request.Content.Amount);
        
        return new ApiResponse<Account>(HttpStatusCode.OK,
            account.AsDto(),
            null);
    }

    public async Task<ApiResponse<Account>> Withdraw(ApiRequest<ChangeBalanceRequest> request)
    {
        var account = await _repo.GetAccount(request.Id);
        if (account is null)
        {
            return new ApiResponse<Account>(HttpStatusCode.NotFound,
                null,
                Messages.NotFound);
        }

        if (request.Content.Amount <= 0)
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest,
                null,
                Messages.NoNegativeAmount);
        }

        if (request.Content.Amount > account.Balance)
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest,
                null,
                Messages.InsufficientBalance);
        }
        
        await _repo.UpdateAccount(account, request.Content.Amount * -1);
        
        return new ApiResponse<Account>(HttpStatusCode.OK,
            account.AsDto(),
            null);
    }
    
    public async Task<ApiResponse<Account>> Transfer(TransferRequest request)
    {
        if (request.Amount <= 0)
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest,
                null,
                Messages.NoNegativeAmount);
        }
        
        var receiver = await _repo.GetAccount(request.ReceiverId);
        if (receiver is null)
        {
            return new ApiResponse<Account>(HttpStatusCode.NotFound,
                null,
                Messages.NotFound);
        }
        
        var sender = await _repo.GetAccount(request.SenderId);
        if (sender is null)
        {
            return new ApiResponse<Account>(HttpStatusCode.NotFound,
                null,
                Messages.NotFound);
        }

        if (request.Amount > sender.Balance)
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest,
                null,
                Messages.InsufficientBalance);
        }
        
        await _repo.UpdateAccount(sender, request.Amount * -1);
        await _repo.UpdateAccount(receiver, request.Amount);

        return new ApiResponse<Account>(HttpStatusCode.OK,
            receiver.AsDto(),
            null);
    }
    
    public async Task<ApiResponse<ConversionResponse>> Convert(ApiRequest<ConversionRequest> request)
    {
        var account = await _repo.GetAccount(request.Id);
        if (account is null)
        {
            return new ApiResponse<ConversionResponse>(HttpStatusCode.NotFound,
                null,
                Messages.NotFound); 
        }

        var response = await _client.GetConversionRates(request.Content.CurrencyType);
        if (response.Content is null)
        {
            return response;
        }

        foreach (var currencyType in response.Content.Data.Keys)
        {
            response.Content.Data[currencyType] *= account.Balance;
        }

        return response;
    }

    private bool ValidateName(string name)
        => Regex.IsMatch(name, NameRegexp);
}