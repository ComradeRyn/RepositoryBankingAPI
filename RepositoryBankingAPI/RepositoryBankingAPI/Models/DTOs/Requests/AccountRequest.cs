namespace RepositoryBankingAPI.Models.DTOs.Requests;

public record AccountRequest<T>(string Id, T Content);