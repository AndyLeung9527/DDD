namespace Ordering.API.Application.Commands;

public class CreateOrderDraftCommand : IRequest<OrderDraftDTO>
{
    public string BuyerId { get; private set; }
    public IEnumerable<BasketItem> Items { get; set; }
    public CreateOrderDraftCommand(string buyerId, IEnumerable<BasketItem> items)
    {
        BuyerId = buyerId;
        Items = items;
    }
}