namespace RepositoryBankingAPI.Models.Records;

public record TransferRequest(decimal Amount, string SenderId, string ReceiverId);