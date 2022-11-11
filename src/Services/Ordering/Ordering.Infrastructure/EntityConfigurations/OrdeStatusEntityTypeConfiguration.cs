namespace Ordering.Infrastructure.EntityConfigurations;

public class OrdeStatusEntityTypeConfiguration
    : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> orderStatusConfiguration)
    {
        orderStatusConfiguration.ToTable("orderstatus");

        orderStatusConfiguration.HasKey(o => o.Id);

        orderStatusConfiguration.Property(o => o.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();

        orderStatusConfiguration.Property(o => o.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}