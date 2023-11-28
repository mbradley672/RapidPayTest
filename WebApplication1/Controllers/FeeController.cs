using Microsoft.AspNetCore.Mvc;
using WebApplication1.PseudoServices;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class FeeController : ControllerBase
{

    private readonly UniversalFeesExchangePseudoService feeService;

    public FeeController(UniversalFeesExchangePseudoService feeService)
    {
        this.feeService = feeService;
    }
    [HttpGet("get")]
    public IActionResult GetTransactionFee(decimal payment)
    {
        var fee = feeService.CurrentFee;

        return Ok(fee);
    }
}