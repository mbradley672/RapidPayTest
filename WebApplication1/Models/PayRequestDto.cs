namespace WebApplication1.Models;

public class PayRequestDto
{
    public string CardNumber { get; set; } = string.Empty;
    public decimal TransactionAmount { get; set; }
}