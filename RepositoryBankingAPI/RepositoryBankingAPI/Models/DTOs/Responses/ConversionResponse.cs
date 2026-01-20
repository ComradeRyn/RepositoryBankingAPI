using System.Net;

namespace RepositoryBankingAPI.Models.DTOs.Responses;

public record ConversionResponse(Dictionary<string, decimal> ConvertedCurrenciesDict);