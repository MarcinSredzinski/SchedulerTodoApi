using System.ComponentModel.DataAnnotations;

namespace Business.Library.Models;

public class ItemToDo : ISoftDeletable
{
    public ItemToDo()
    {
    }
    public ItemToDo(ItemToDoDto itemToDoDto)
    {
        Id = Guid.NewGuid();
        Name = itemToDoDto.Name;
        IsChecked = itemToDoDto.IsChecked;
        IsDeleted = false;
    }
    [Key]
    public Guid Id { get; init; }
    [MaxLength(100)] 
    public string? Name { get; set; }
    public bool IsChecked { get; set; }
    public DateTimeOffset? RemindOn { get; set; }
    
    public DateTimeOffset? Deadline { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }
}