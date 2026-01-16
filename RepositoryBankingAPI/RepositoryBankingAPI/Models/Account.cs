namespace RepositoryBankingAPI.Models;

public class Account
{
    private int InteralId;
    public string Id { get; init; }
    public string HolderName{ get; init; }
    public decimal Balance { get; set; }
}