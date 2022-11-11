namespace Ordering.Infrastructure.EntityConfigurations;

public class OrderItemEntityTypeConfiguration
    : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> orderItemConfiguration)
    {
        orderItemConfiguration.ToTable("orderItems");

        orderItemConfiguration.HasKey(o => o.Id);

        orderItemConfiguration.Ignore(b => b.DomainEvents);

        orderItemConfiguration.Property<int>("orderId")
            .IsRequired();

        orderItemConfiguration
            .Property<decimal>("_discount")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Discount")
            .IsRequired();

        orderItemConfiguration.Property<int>("ProductId")
            .IsRequired();

        orderItemConfiguration
            .Property<string>("_productName")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("ProductName")
            .IsRequired();

        orderItemConfiguration
            .Property<decimal>("_unitPrice")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("UnitPrice")
            .IsRequired();

        orderItemConfiguration
            .Property<int>("_units")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Units")
            .IsRequired();

        orderItemConfiguration
            .Property<string>("_pictureUrl")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("PictureUrl")
            .IsRequired(false);
    }
}
