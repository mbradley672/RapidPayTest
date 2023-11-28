using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Extensions;

namespace WebApplication1.Controllers;

[ApiController, Route("[controller]")]
public class AccountController : Controller
{
    private readonly RapidPayDbContext context;

    public AccountController(RapidPayDbContext context)
    {
        this.context = context;
    }
    
    [HttpPut("Create")]
    public async Task<IActionResult> CreateAccount([FromBody]AccountCreationDto request)
    {
        var account = request.FromDto();
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();

        return Ok($"Account Created with id: {account.Id}");
    }

    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> GetAccount(int id)
    {
        var accountFromDb = await context.Accounts.SingleOrDefaultAsync(x => x.Id == id);
        return Ok(JsonSerializer.Serialize(accountFromDb));
    }

    [HttpGet("login")]
    public async Task<IActionResult> GetAccount([FromBody]AccountLoginDto request)
    {
        var accountFromDb =
            await context.Accounts.SingleOrDefaultAsync(x =>
                x.Username == request.Username && x.Password == request.Password);

        if (accountFromDb is null)
        {
            return BadRequest();
        }

        return Ok(JsonSerializer.Serialize(accountFromDb.Id));
    }
}

