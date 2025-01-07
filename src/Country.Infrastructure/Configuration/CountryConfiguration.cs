using Country.Domain.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Country.Infrastructure.Configuration
{
    internal sealed class CountryConfiguration : IEntityTypeConfiguration<Domain.Countries.Country>
    {
        public void Configure(EntityTypeBuilder<Domain.Countries.Country> builder)
        {
            builder.HasKey(country => country.Id);

            builder.Property(country => country.Name)
                .HasMaxLength(80);

            builder.Property(country => country.IsoCode)
                .HasConversion(code => code.Value,
                stringValue => new IsoCode(stringValue));

            builder.Property(country => country.Code)
                .HasConversion(code => code.Value,
                stringValue => new CountryCode(stringValue));

            builder.HasMany(country => country.Operators)
                .WithOne(op => op.Country)
                .HasForeignKey(op => op.CountryId);
        }
    }
}
