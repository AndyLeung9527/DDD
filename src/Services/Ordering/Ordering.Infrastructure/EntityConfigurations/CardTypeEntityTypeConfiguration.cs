namespace Ordering.Infrastructure.EntityConfigurations;

public class CardTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CardType>
{
    public void Configure(EntityTypeBuilder<CardType> cardTypeConfiguration)
    {
        cardTypeConfiguration.ToTable("cardtypes");

        cardTypeConfiguration.HasKey(ct => ct.Id);

        cardTypeConfiguration.Property(ct => ct.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();

        cardTypeConfiguration.Property(ct => ct.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}