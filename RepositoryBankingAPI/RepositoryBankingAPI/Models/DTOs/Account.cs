namespace RepositoryBankingAPI.Models.DTOs;

public record Account(string Id, 
    string HolderName, 
    decimal Amount);