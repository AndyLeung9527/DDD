namespace Ordering.Infrastructure.EntityConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> orderConfiguration)
    {
        orderConfiguration.ToTable("orders");

        orderConfiguration.HasKey(o => o.Id);

        orderConfiguration.Ignore(b => b.DomainEvents);

        orderConfiguration.OwnsOne(o => o.Address, a =>
        {
            a.Property<int>("OrderId");
            a.WithOwner();
        });

        orderConfiguration
            .Property<int?>("_buyerId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("BuyerId")
            .IsRequired(false);

        orderConfiguration
            .Property<DateTime>("_orderDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("OrderDate")
            .IsRequired();

        orderConfiguration
            .Property<int>("_orderStatusId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("OrderStatusId")
            .IsRequired();

        orderConfiguration
            .Property<int?>("_paymentMethodId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("PaymentMethodId")
            .IsRequired(false);

        orderConfiguration.Property<string>("Description").IsRequired(false);

        var navigation = orderConfiguration.Metadata.FindNavigation(nameof(Order.OrderItems));

        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

        orderConfiguration.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey("_paymentMethodId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        orderConfiguration.HasOne<Buyer>()
            .WithMany()
            .IsRequired(false)
            .HasForeignKey("_buyerId");

        orderConfiguration.HasOne(o => o.OrderStatus)
            .WithMany()
            .HasForeignKey("_orderStatusId");
    }
}
