namespace Ordering.UnitTests;

public class OrdersWebApiTest
{
    readonly Mock<IMediator> _mediatorMock;
    readonly Mock<IIdentityService> _identityMock;
    readonly Mock<ILogger<OrdersController>> _loggerMock;

    public OrdersWebApiTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _identityMock = new Mock<IIdentityService>();
        _loggerMock = new Mock<ILogger<OrdersController>>();
    }

    [Fact]
    public async Task Cancel_order_with_requestId_success()
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CancelOrderCommand, bool>>(), default(CancellationToken)))
            .Returns(Task.FromResult(true));

        //Act
        var ordersController = new OrdersController(_mediatorMock.Object, _identityMock.Object, _loggerMock.Object);
        var actionResult = await ordersController.CancelOrderAsync(new CancelOrderCommand(1), Guid.NewGuid().ToString()) as OkResult;

        //Assert
        Assert.Equal(actionResult.StatusCode, (int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task Cancel_order_bad_request()
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CancelOrderCommand, bool>>(), default(CancellationToken)))
            .Returns(Task.FromResult(true));

        //Act
        var ordersController = new OrdersController(_mediatorMock.Object, _identityMock.Object, _loggerMock.Object);
        var actionResult = await ordersController.CancelOrderAsync(new CancelOrderCommand(1), String.Empty) as BadRequestResult;

        //Assert
        Assert.Equal(actionResult.StatusCode, (int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Ship_order_with_requestId_success()
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<ShipOrderCommand, bool>>(), default(CancellationToken)))
            .Returns(Task.FromResult(true));

        //Act
        var ordersController = new OrdersController(_mediatorMock.Object, _identityMock.Object, _loggerMock.Object);
        var actionResult = await ordersController.ShipOrderAsync(new ShipOrderCommand(1), Guid.NewGuid().ToString()) as OkResult;

        //Assert
        Assert.Equal(actionResult.StatusCode, (int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task Ship_order_bad_request()
    {
        //Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CancelOrderCommand, bool>>(), default(CancellationToken)))
            .Returns(Task.FromResult(true));

        //Act
        var ordersController = new OrdersController(_mediatorMock.Object, _identityMock.Object, _loggerMock.Object);
        var actionResult = await ordersController.ShipOrderAsync(new ShipOrderCommand(1), String.Empty) as BadRequestResult;

        //Assert
        Assert.Equal(actionResult.StatusCode, (int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_order_success()
    {
        //Arrange
        var fakeOrderId = 123;
        var fakeDynamicResult = new Order();
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOrderByIdQuery>(), default(CancellationToken)))
            .Returns(Task.FromResult(fakeDynamicResult));

        //Action
        var ordersController = new OrdersController(_mediatorMock.Object, _identityMock.Object, _loggerMock.Object);
        var actionResult = await ordersController.GetOrderAsync(fakeOrderId) as OkObjectResult;

        //Assert
        Assert.Equal(actionResult.StatusCode, (int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_orders_success()
    {
        //Arrange
        var fakeUserId = Guid.Empty.ToString();
        var fakeDynamicResult = Enumerable.Empty<OrderSummary>();
        _identityMock.Setup(x => x.GetUserIdentity())
            .Returns(fakeUserId);
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetOrdersFromUserQuery>(), default(CancellationToken)))
            .Returns(Task.FromResult(fakeDynamicResult));

        //Action
        var ordersController = new OrdersController(_mediatorMock.Object, _identityMock.Object, _loggerMock.Object);
        var actionResult = await ordersController.GetOrdersAsync();

        //Assert
        Assert.Equal((actionResult.Result as OkObjectResult).StatusCode, (int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_cardTypes_success()
    {
        //Arrange
        var fakeDynamicResult = Enumerable.Empty<CardType>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetCardTypesQuery>(), default(CancellationToken)))
            .Returns(Task.FromResult(fakeDynamicResult));

        //Action
        var ordersController = new OrdersController(_mediatorMock.Object, _identityMock.Object, _loggerMock.Object);
        var actionResult = await ordersController.GetCardTypesAsync();

        //Assert
        Assert.Equal((actionResult.Result as OkObjectResult).StatusCode, (int)HttpStatusCode.OK);
    }
}
