namespace Ordering.API.Application.Commands;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Ordering.API.Application.Models.Order>
{
    readonly string _connectionString;
    public GetOrderByIdQueryHandler(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionString"];
    }

    public async Task<Ordering.API.Application.Models.Order> Handle(GetOrderByIdQuery command, CancellationToken cancellationToken)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var result = await connection.QueryAsync<dynamic>(
            @"select o.Id as ordernumber, o.OrderDate as date, o.Description as description,
                     o.Address_City as city, o.Address_Country as country, o.Address_State as state, o.Address_Street as street, o.Address_ZipCode as zipcode,
                     os.Name as status,
                     oi.ProductName as productname, oi.Units as units, oi.UnitPrice as unitprice, oi.PictureUrl as pictureurl
                     FROM ordering.Orders o
                     LEFT JOIN ordering.Orderitems oi ON o.Id = oi.orderid
                     LEFT JOIN ordering.orderstatus os on o.OrderStatusId = os.Id
                     WHERE o.Id=@id"
                , new { command.OrderNumber }
            );

        if (result.AsList().Count == 0)
            throw new KeyNotFoundException();

        return MapOrderItems(result);
    }

    private Ordering.API.Application.Models.Order MapOrderItems(dynamic result)
    {
        var order = new Ordering.API.Application.Models.Order
        {
            ordernumber = result[0].ordernumber,
            date = result[0].date,
            status = result[0].status,
            description = result[0].description,
            street = result[0].street,
            city = result[0].city,
            zipcode = result[0].zipcode,
            country = result[0].country,
            orderitems = new List<Ordering.API.Application.Models.Orderitem>(),
            total = 0
        };

        foreach (dynamic item in result)
        {
            var orderitem = new Ordering.API.Application.Models.Orderitem
            {
                productname = item.productname,
                units = item.units,
                unitprice = (double)item.unitprice,
                pictureurl = item.pictureurl
            };

            order.total += item.units * item.unitprice;
            order.orderitems.Add(orderitem);
        }

        return order;
    }
}
