using System.Net;
using System.Text.RegularExpressions;
using RepositoryBankingAPI.Interfaces;
using RepositoryBankingAPI.Models.Constants;
using RepositoryBankingAPI.Models.DTOs.Requests;
using RepositoryBankingAPI.Models.DTOs.Responses;
using Account = RepositoryBankingAPI.Models.DTOs.Responses.Account;

namespace RepositoryBankingAPI.Services;

public class AccountsService
{
    private const string NameRegexp = @"([A-Z][a-z]+)\s(([A-Z][a-z]*)\s)?([A-Z][a-z]+)";
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyClient _client;

    public AccountsService(IAccountRepository accountRepository, ICurrencyClient client)
    {
        _accountRepository = accountRepository;
        _client = client;
    }

    public async Task<ApiResponse<Account>> GetAccount(string id)
    {
        var account = await _accountRepository.GetAccount(id);
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

    public async Task<ApiResponse<Account>> Deposit(AccountRequest<ChangeBalanceRequest> request)
    {
        var account = await _accountRepository.GetAccount(request.Id);
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

        await _accountRepository.UpdateAccount(account, request.Content.Amount);
        
        return new ApiResponse<Account>(HttpStatusCode.OK,
            account.AsDto(),
            null);
    }

    public async Task<ApiResponse<Account>> Withdraw(AccountRequest<ChangeBalanceRequest> request)
    {
        var account = await _accountRepository.GetAccount(request.Id);
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
        
        await _accountRepository.UpdateAccount(account, request.Content.Amount * -1);
        
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
        
        var receiver = await _accountRepository.GetAccount(request.ReceiverId);
        if (receiver is null)
        {
            return new ApiResponse<Account>(HttpStatusCode.NotFound,
                null,
                Messages.NotFound);
        }
        
        var sender = await _accountRepository.GetAccount(request.SenderId);
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
        
        await _accountRepository.UpdateAccount(sender, request.Amount * -1);
        await _accountRepository.UpdateAccount(receiver, request.Amount);

        return new ApiResponse<Account>(HttpStatusCode.OK,
            receiver.AsDto(),
            null);
    }
    
    public async Task<ApiResponse<ConversionResponse>> Convert(AccountRequest<ConversionRequest> request)
    {
        var account = await _accountRepository.GetAccount(request.Id);
        if (account is null)
        {
            return new ApiResponse<ConversionResponse>(HttpStatusCode.NotFound,
                null,
                Messages.NotFound); 
        }

        var response = await _client.GetConversionRates(request.Content.CurrencyType);
        if (!response.IsSuccess)
        {
            return new ApiResponse<ConversionResponse>(response.StatusCode,
                null,
                response.ErrorMessage);
        }

        var convertedCurrencies = response.ConversionRates;
        foreach (var currencyType in convertedCurrencies!.Keys)
        {
            convertedCurrencies[currencyType] *= account.Balance;
        }

        return new ApiResponse<ConversionResponse>(HttpStatusCode.OK,
            convertedCurrencies.AsDto(),
            null);
    }

    private bool ValidateName(string name)
        => Regex.IsMatch(name, NameRegexp);
}