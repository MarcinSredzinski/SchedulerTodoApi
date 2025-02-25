namespace Business.Library.Models;

public class CurrencyExchangeRateTableModel
{
    public ExchangeTable[]? ExchangeTable { get; set; }
}

public class ExchangeTable
{
    public string? Table { get; set; }
    public string? No { get; set; }
    public string? EffectiveDate { get; set; }
    public Rate[]? Rates { get; set; }
}

public class Rate
{
    public string? Currency { get; set; }
    public string? Code { get; set; }
    public decimal Mid { get; set; }
}