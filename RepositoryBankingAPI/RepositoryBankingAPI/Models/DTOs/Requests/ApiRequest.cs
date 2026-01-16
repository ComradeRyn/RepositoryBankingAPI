namespace RepositoryBankingAPI.Models.DTOs.Requests;

public record ApiRequest<T>(string Id, T Request);