namespace Ordering.Infrastructure;

public class OrderingContext : DbContext, IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<PaymentMethod> Payments { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<CardType> CardTypes { get;set; }
    public DbSet<OrderStatus> OrderStatus { get; set; }

    private readonly IMediator _mediator;

    public OrderingContext(DbContextOptions<OrderingContext> options) : base(options)
    {
    }
    public OrderingContext(DbContextOptions<OrderingContext> options,IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CardTypeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrdeStatusEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this);

        var result = await base.SaveChangesAsync(cancellationToken);

        return true;
    }
}
