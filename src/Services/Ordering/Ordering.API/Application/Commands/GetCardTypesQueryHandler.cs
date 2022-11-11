namespace Ordering.API.Application.Commands;

public class GetCardTypesQueryHandler : IRequestHandler<GetCardTypesQuery, IEnumerable<CardType>>
{
    readonly string _connectionString;
    public GetCardTypesQueryHandler(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionString"];
    }
    public async Task<IEnumerable<CardType>> Handle(GetCardTypesQuery command, CancellationToken cancellationToken)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        return await connection.QueryAsync<CardType>("SELECT * FROM ordering.cardtypes");
    }
}