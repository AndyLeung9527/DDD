namespace Ordering.API.Application.Commands;

public class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand, bool>
{
    readonly IOrderRepository _orderRepository;

    public ShipOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(ShipOrderCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
        if(orderToUpdate == null)
        {
            return false;
        }

        orderToUpdate.SetShippedStatus();
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
