namespace RepositoryBankingAPI.Services;

public static class Messages
{
    public const string NotFound = "Requested object could not be found";
    public const string InsufficientBalance = "Requested amount must be less or equal to current balance";
    public const string RequirePositiveAmount = "Requested amount must be positive";
    public const string InvalidName = "Requested amount must be less than or equal to current balance";
}