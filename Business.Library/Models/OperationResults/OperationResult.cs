namespace Business.Library.Models.OperationResults
{
    public record OperationResult(bool IsSuccessful, string? Message);
    public record OperationSuccessfulResult():OperationResult(true, null);
}