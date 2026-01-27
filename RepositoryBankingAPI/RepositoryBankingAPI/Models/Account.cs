namespace RepositoryBankingAPI.Models;

public class Account
{
    private int InternalId;
    public required string Id { get; init; }
    public required string HolderName { get; init; }
    public decimal Balance { get; set; }
}