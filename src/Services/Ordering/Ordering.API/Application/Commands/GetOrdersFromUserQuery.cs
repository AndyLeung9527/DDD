namespace Ordering.API.Application.Commands;

public class GetOrdersFromUserQuery : IRequest<IEnumerable<OrderSummary>>
{
    [DataMember]
    public Guid UserId { get; set; }
    public GetOrdersFromUserQuery()
    {
    }
    public GetOrdersFromUserQuery(Guid userId)
    {
        UserId = userId;
    }
}