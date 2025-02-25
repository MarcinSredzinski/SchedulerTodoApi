namespace Business.Library.Models;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
    DateTimeOffset? DeletedOnUtc { get; set; }
}