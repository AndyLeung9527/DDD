namespace Ordering.API.Application.Models;

public record OrderDraftDTO
{
    public IEnumerable<OrderItemDTO> OrderItems { get; init; }
    public decimal Total { get; init; }

    public static OrderDraftDTO FromOrder(Ordering.Domain.AggregatesModel.OrderAggregate.Order order)
    {
        return new OrderDraftDTO()
        {
            OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
            {
                Discount = oi.GetCurrentDiscount(),
                ProductId = oi.ProductId,
                UnitPrice = oi.GetUnitPrice(),
                PictureUrl = oi.GetPictureUri(),
                Units = oi.GetUnits(),
                ProductName = oi.GetOrderItemProductName()
            }),
            Total = order.GetTotal()
        };
    }

}
