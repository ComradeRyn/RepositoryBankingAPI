namespace RepositoryBankingAPI.Models;

public class Account
{
    public string Id { get; init; }
    public string HolderName{ get; init; }
    public decimal Balance { get; set; }
}