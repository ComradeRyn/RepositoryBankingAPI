using System.Net;

namespace RepositoryBankingAPI.Models.DTOs.Responses;

public record CurrencyApiResponse(Dictionary<string, decimal> Data);