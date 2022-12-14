namespace Ordering.API.Application.Commands;

public class ShipOrderCommand : IRequest<bool>
{
    public int OrderNumber { get; private set; }

    public ShipOrderCommand(int orderNumber)
    {
        OrderNumber = orderNumber;
    }
}
