using System.ComponentModel.DataAnnotations;

namespace Business.Library.Models;

public class ItemToDoDto
{
    [MaxLength(100)]
    public string? Name { get; set; }
    public bool IsChecked { get; set; }
}