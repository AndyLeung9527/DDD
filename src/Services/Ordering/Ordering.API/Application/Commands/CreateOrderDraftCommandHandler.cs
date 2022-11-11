namespace Ordering.API.Application.Commands;

public class CreateOrderDraftCommandHandler : IRequestHandler<CreateOrderDraftCommand, OrderDraftDTO>
{
    readonly IIdentityService _identityService;
    readonly IMediator _mediator;
    public CreateOrderDraftCommandHandler(IIdentityService identityService, IMediator mediator)
    {
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public Task<OrderDraftDTO> Handle(CreateOrderDraftCommand command, CancellationToken cancellationToken)
    {
        var order = Ordering.Domain.AggregatesModel.OrderAggregate.Order.NewDraft();
        var orderItems = command.Items.Select(i => i.ToOrderItemDTO());
        foreach (var item in orderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
        }

        return Task.FromResult(OrderDraftDTO.FromOrder(order));
    }
}