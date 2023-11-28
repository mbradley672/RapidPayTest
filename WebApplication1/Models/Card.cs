using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public class Card
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int CardHolderAccountId { get; set; }
    [MinLength(15), MaxLength(15)] 
    public string CardNumber { get; set; } = GenerateCreditCardNumber();

    public decimal CreditLimit { get; set; } = 15000m;
    public decimal Balance { get; set; } = 0m;
    [NotMapped]
    public virtual decimal AvailableCredit => CreditLimit - Balance;


    public static string GenerateCreditCardNumber()
    {
        var random = new Random();
        const int length = 15;
        const string chars = "0123456789";

        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

public class AccountLoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}