using Country.Domain.Operators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Country.Infrastructure.Configuration
{
    internal sealed class OperatorConfiguration : IEntityTypeConfiguration<Operator>
    {
        public void Configure(EntityTypeBuilder<Operator> builder)
        {
            builder.HasKey(op => op.Id);
            builder.Property(op => op.Name)
                .HasMaxLength(130);
            builder.Property(op => op.Code)
                .HasMaxLength(100);
        }
    }
}
