namespace Ordering.API.Application.Commands;

public class GetOrderByIdQuery : IRequest<Ordering.API.Application.Models.Order>
{
    [DataMember]
    public int OrderNumber { get; set; }
    public GetOrderByIdQuery()
    {
    }
    public GetOrderByIdQuery(int orderId)
    {
        OrderNumber = orderId;
    }
}
