using System.Net;
using System.Text.RegularExpressions;
using RepositoryBankingAPI.Interfaces;
using RepositoryBankingAPI.Models.Constants;
using RepositoryBankingAPI.Models.DTOs;
using RepositoryBankingAPI.Models.DTOs.Requests;
using RepositoryBankingAPI.Models.DTOs.Responses;

namespace RepositoryBankingAPI.Services;

public class AccountsService
{
    private const string NameRegexp = @"([A-Z][a-z]+)\s(([A-Z][a-z]*)\s)?([A-Z][a-z]+)";
    private readonly IAccountsRepository _accountsRepository;
    private readonly ICurrencyClient _currencyClient;

    public AccountsService(IAccountsRepository accountsRepository, ICurrencyClient currencyClient)
    {
        _accountsRepository = accountsRepository;
        _currencyClient = currencyClient;
    }

    public async Task<ApiResponse<Account>> GetAccount(string id)
    {
        var account = await _accountsRepository.GetAccount(id);
        if (account is null)
        {
            return new ApiResponse<Account>(HttpStatusCode.NotFound, Messages.NotFound);
        }
        
        return new ApiResponse<Account>(account.AsDto());
    }

    public async Task<ApiResponse<Account>> CreateAccount(CreationRequest request)
    {
        if (!ValidateName(request.Name))
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest, Messages.InvalidName);
        }
        
        var newAccount = await _accountsRepository.AddAccount(request.Name);
        
        return new ApiResponse<Account>(newAccount.AsDto());
    }

    public async Task<ApiResponse<Account>> Deposit(AccountRequest<ChangeBalanceRequest> request)
    {
        var account = await _accountsRepository.GetAccount(request.Id);
        if (account is null)
        {
            return new ApiResponse<Account>(HttpStatusCode.NotFound, Messages.NotFound);
        }

        if (request.Content.Amount <= 0)
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest, Messages.NoNegativeAmount);
        }

        await _accountsRepository.UpdateAccount(account, request.Content.Amount);
        
        return new ApiResponse<Account>(account.AsDto());
    }

    public async Task<ApiResponse<Account>> Withdraw(AccountRequest<ChangeBalanceRequest> request)
    {
        var account = await _accountsRepository.GetAccount(request.Id);
        if (account is null)
        {
            return new ApiResponse<Account>(HttpStatusCode.NotFound, Messages.NotFound);
        }

        if (request.Content.Amount <= 0)
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest, Messages.NoNegativeAmount);
        }

        if (request.Content.Amount > account.Balance)
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest, Messages.InsufficientBalance);
        }
        
        await _accountsRepository.UpdateAccount(account, request.Content.Amount * -1);
        
        return new ApiResponse<Account>(account.AsDto());
    }
    
    public async Task<ApiResponse<Account>> Transfer(TransferRequest request)
    {
        var receiver = await _accountsRepository.GetAccount(request.ReceiverId);
        if (receiver is null)
        {
            return new ApiResponse<Account>(HttpStatusCode.NotFound, Messages.NotFound);
        }
        
        var sender = await _accountsRepository.GetAccount(request.SenderId);
        if (sender is null)
        {
            return new ApiResponse<Account>(HttpStatusCode.NotFound, Messages.NotFound);
        }
        
        if (request.Amount <= 0)
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest, Messages.NoNegativeAmount);
        }

        if (request.Amount > sender.Balance)
        {
            return new ApiResponse<Account>(HttpStatusCode.BadRequest, Messages.InsufficientBalance);
        }
        
        await _accountsRepository.UpdateAccount(sender, request.Amount * -1);
        await _accountsRepository.UpdateAccount(receiver, request.Amount);

        return new ApiResponse<Account>(sender.AsDto());
    }
    
    public async Task<ApiResponse<ConversionResponse>> Convert(AccountRequest<ConversionRequest> request)
    {
        var account = await _accountsRepository.GetAccount(request.Id);
        if (account is null)
        {
            return new ApiResponse<ConversionResponse>(HttpStatusCode.NotFound, Messages.NotFound); 
        }

        var response = await _currencyClient.GetConversionRates(request.Content.CurrencyType);
        if (!response.IsSuccess)
        {
            return new ApiResponse<ConversionResponse>((HttpStatusCode)response.ErrorCode!, response.ErrorMessage!);
        }

        var convertedCurrencies = response.ConversionRates;
        foreach (var currencyType in convertedCurrencies!.Keys)
        {
            convertedCurrencies[currencyType] *= account.Balance;
        }

        return new ApiResponse<ConversionResponse>(new ConversionResponse(convertedCurrencies));
    }

    private bool ValidateName(string name)
        => Regex.IsMatch(name, NameRegexp);
}