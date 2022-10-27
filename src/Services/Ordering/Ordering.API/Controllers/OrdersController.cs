namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    readonly ILogger<OrdersController> _logger;
    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }

    [Route("index")]
    [HttpGet]
    public IActionResult Index()
    {
        _logger.LogInformation("teststesetsetesttestest");
        return Ok("Hello DDD");
    }
}
