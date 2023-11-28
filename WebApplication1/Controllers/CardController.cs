using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Extensions;
using WebApplication1.PseudoServices;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class CardController : ControllerBase
{
    private readonly RapidPayDbContext context;
    private readonly UniversalFeesExchangePseudoService feeService;

    public CardController(RapidPayDbContext context, UniversalFeesExchangePseudoService feeService)
    {
        this.context = context;
        this.feeService = feeService;
    }
    
    [HttpGet("{cardNumber}")]
    public async Task<IActionResult> GetCardInfo(string cardNumber)
    {
        var cardFromDb = await context.Cards.SingleOrDefaultAsync(x => x.CardNumber == cardNumber);
        if (cardFromDb == null) return NotFound("The account was not found");

        return Ok(JsonSerializer.Serialize(cardFromDb));
    }

    [HttpPut("create")]
    public async Task<IActionResult> RequestAccount([FromBody] CardRequestDto newCardAccount)
    {
        var newAccount = newCardAccount.FromDto();

        await context.Cards.AddAsync(newAccount);
        await context.SaveChangesAsync();
        
        return Ok(JsonSerializer.Serialize(newAccount));
    }

    [HttpPost("pay")]
    public async Task<IActionResult> PayWithAccount([FromBody] PayRequestDto requestDto)
    {
        var accountFromDb = await context.Cards.SingleOrDefaultAsync(x => x.CardNumber == requestDto.CardNumber);
        if (accountFromDb == null) return NotFound("The account is not found");
        if (requestDto.TransactionAmount > accountFromDb.AvailableCredit)
        {
            return BadRequest("Your available balance is less than the Transaction amount");
        }

        var fee = GetFee(requestDto.TransactionAmount);
        accountFromDb.Balance += requestDto.TransactionAmount + fee;
        context.Cards.Update(accountFromDb);
        await context.SaveChangesAsync();

        feeService.UpdateLastFee(fee);

        return Ok($"Transaction successful your new balance is {accountFromDb.Balance:0.00#} " +
                  $"with an available credit of {accountFromDb.AvailableCredit:0.00#}");
    }

    private decimal GetFee(decimal amount)
    {
        return feeService.GetFee(amount);
    }
}