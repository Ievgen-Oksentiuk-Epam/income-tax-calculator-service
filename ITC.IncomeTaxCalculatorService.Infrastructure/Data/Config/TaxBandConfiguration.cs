using ITC.IncomeTaxCalculatorService.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITC.IncomeTaxCalculatorService.Infrastructure.Data.Config
{
    internal class TaxBandConfiguration : IEntityTypeConfiguration<TaxBand>
    {
        public void Configure(EntityTypeBuilder<TaxBand> builder)
        {
            builder.HasKey(tb => tb.Id);

            builder.Property(tb => tb.Name).IsRequired();
            builder.Property(tb => tb.LowerLimit).IsRequired();
            builder.Property(tb => tb.TaxRate).IsRequired();

            builder.HasData(
                new TaxBand
                {
                    Id = 1,
                    Name = "A",
                    LowerLimit = 0,
                    UpperLimit = 5000,
                    TaxRate = 0
                },
                new TaxBand
                {
                    Id = 2,
                    Name = "B",
                    LowerLimit = 5000,
                    UpperLimit = 20000,
                    TaxRate = 20
                },
                new TaxBand
                {
                    Id = 3,
                    Name = "C",
                    LowerLimit = 20000,
                    TaxRate = 40
                }
                );
        }
    }
}
