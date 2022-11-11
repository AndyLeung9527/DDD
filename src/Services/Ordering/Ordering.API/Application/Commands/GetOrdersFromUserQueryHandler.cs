namespace Ordering.API.Application.Commands;

public class GetOrdersFromUserQueryHandler : IRequestHandler<GetOrdersFromUserQuery, IEnumerable<OrderSummary>>
{
    readonly string _connectionString;
    public GetOrdersFromUserQueryHandler(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionString"];
    }

    public async Task<IEnumerable<OrderSummary>> Handle(GetOrdersFromUserQuery command, CancellationToken cancellationToken)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        return await connection.QueryAsync<OrderSummary>(@"SELECT o.[Id] as ordernumber,o.[OrderDate] as [date],os.[Name] as [status], SUM(oi.units*oi.unitprice) as total
                    FROM [ordering].[Orders] o
                    LEFT JOIN[ordering].[orderitems] oi ON  o.Id = oi.orderid 
                    LEFT JOIN[ordering].[orderstatus] os on o.OrderStatusId = os.Id                     
                    LEFT JOIN[ordering].[buyers] ob on o.BuyerId = ob.Id
                    WHERE ob.IdentityGuid = @userId
                    GROUP BY o.[Id], o.[OrderDate], os.[Name] 
                    ORDER BY o.[Id]", new { command.UserId });
    }
}
