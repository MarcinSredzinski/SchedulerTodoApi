using System.ComponentModel.DataAnnotations;

namespace Business.Library.Models;

public class ApiKey
{
    [Key]
    public int Id { get; set; }
    public string Key { get; set; }
    public DateTime? Expiration { get; set; }
}