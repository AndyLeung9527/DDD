namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    readonly IMediator _mediator;
    readonly IIdentityService _identityService;
    readonly ILogger<OrdersController> _logger;
    public OrdersController(IMediator mediator, IIdentityService identityService, ILogger<OrdersController> logger)
    {
        _mediator = mediator;
        _identityService = identityService;
        _logger = logger;
    }

    [Route("cancel")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CancelOrderAsync([FromBody] CancelOrderCommand command, [FromHeader(Name = "x-requestid")] string requestId)
    {
        bool commandResult = false;

        if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {
            var requestCancelOrder = new IdentifiedCommand<CancelOrderCommand, bool>(command, guid);

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                requestCancelOrder.GetGenericTypeName(),
                nameof(requestCancelOrder.Command.OrderNumber),
                requestCancelOrder.Command.OrderNumber,
                requestCancelOrder);

            commandResult = await _mediator.Send(requestCancelOrder);
        }

        if (!commandResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [Route("ship")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ShipOrderAsync([FromBody] ShipOrderCommand command, [FromHeader(Name = "x-requestid")] string requestId)
    {
        bool commandResult = false;

        if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {
            var requestShipOrder = new IdentifiedCommand<ShipOrderCommand, bool>(command, guid);

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@command})",
                requestShipOrder.GetGenericTypeName(),
                nameof(requestShipOrder.Command.OrderNumber),
                requestShipOrder.Command.OrderNumber,
                requestShipOrder);

            commandResult = await _mediator.Send(requestShipOrder);
        }

        if (!commandResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [Route("{orderId:int}")]
    [HttpGet]
    [ProducesResponseType(typeof(Ordering.API.Application.Models.Order), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetOrderAsync(int orderId)
    {
        try
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(orderId));

            return Ok(order);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderSummary>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderSummary>>> GetOrdersAsync()
    {
        var userid = _identityService.GetUserIdentity();
        var orders = await _mediator.Send(new GetOrdersFromUserQuery(Guid.Parse(userid)));

        return Ok(orders);
    }

    [Route("cardtypes")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CardType>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<CardType>>> GetCardTypesAsync()
    {
        var cardTypes = await _mediator.Send(new GetCardTypesQuery());

        return Ok(cardTypes);
    }

    [Route("draft")]
    [HttpPost]
    public async Task<ActionResult<OrderDraftDTO>> CreateOrderDraftFromBasketDataAsync([FromBody] CreateOrderDraftCommand createOrderDraftCommand)
    {
        _logger.LogInformation(
            "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            createOrderDraftCommand.GetGenericTypeName(),
            nameof(createOrderDraftCommand.BuyerId),
            createOrderDraftCommand.BuyerId,
            createOrderDraftCommand);

        return await _mediator.Send(createOrderDraftCommand);
    }
}