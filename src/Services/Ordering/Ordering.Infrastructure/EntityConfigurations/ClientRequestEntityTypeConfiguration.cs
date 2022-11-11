namespace Ordering.Infrastructure.EntityConfigurations;

public class ClientRequestEntityTypeConfiguration
    : IEntityTypeConfiguration<ClientRequest>
{
    public void Configure(EntityTypeBuilder<ClientRequest> requestConfiguration)
    {
        requestConfiguration.ToTable("requests");
        requestConfiguration.HasKey(cr => cr.Id);
        requestConfiguration.Property(cr => cr.Name).IsRequired();
        requestConfiguration.Property(cr => cr.Time).IsRequired();
    }
}
